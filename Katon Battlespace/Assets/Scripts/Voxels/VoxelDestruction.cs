using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Game.Voxels
{
    public class VoxelDestruction : MonoBehaviour
    {
        private static VoxelWorldComponent worldComponentPrefab = null;

        private static VoxelWorldComponent component;
        private static List<VoxelWorldComponent> worldList = new List<VoxelWorldComponent>();

        private static NativeList<int3> posStack;
        private static NativeList<int> indexStack;

        private static NativeArray<int> mask;
        private static NativeList<int> partsCount;
        private static NativeArray<int3> minArray;
        private static NativeArray<int3> maxArray;

        private static int mainIndex;

        private static JobHandle handle;

        private static readonly ProfilerMarker floodMarker = new ProfilerMarker("FloodFill");
        private static readonly ProfilerMarker boundsMarker = new ProfilerMarker("FindBounds");
        private static readonly ProfilerMarker createWorldsMarker = new ProfilerMarker("CreateNewWorlds");
        private static readonly ProfilerMarker updateWorldMarker = new ProfilerMarker("UpdateWorlds");

        private void Awake()
        {
            posStack = new NativeList<int3>(Allocator.Persistent);
            indexStack = new NativeList<int>(Allocator.Persistent);
            partsCount = new NativeList<int>(Allocator.Persistent);

            worldComponentPrefab = Resources.Load<VoxelWorldComponent>("Prefabs/World");
        }

        private void OnDestroy()
        {
            posStack.Dispose();
            indexStack.Dispose();
            partsCount.Dispose();
        }

        public static void DestructWorld(VoxelWorldComponent component)
        {
            VoxelDestruction.component = component;

            mask = new NativeArray<int>(component.world.Length, Allocator.TempJob);
            partsCount.Clear();

            FloodFill();

            minArray = new NativeArray<int3>(partsCount.Length, Allocator.TempJob);
            maxArray = new NativeArray<int3>(partsCount.Length, Allocator.TempJob);

            FindBounds();

            CreateNewWorlds();

            UpdateWorlds();

            mask.Dispose();
            minArray.Dispose();
            maxArray.Dispose();
        }

        public static int GetPartsCount(VoxelWorldComponent component)
        {
            VoxelDestruction.component = component;

            mask = new NativeArray<int>(component.world.Length, Allocator.TempJob);
            partsCount.Clear();

            floodMarker.Begin();
            FloodFill();
            floodMarker.End();

            mask.Dispose();

            return partsCount.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FloodFill()
        {
            floodMarker.Begin();

            FloodFillJob floodJob = new FloodFillJob()
            {
                voxels = component.world[VoxelType.Content],
                mask = mask,
                posStack = posStack,
                indexStack = indexStack,

                partsCount = partsCount,

                size = component.world.size,
                mul = component.world.mul
            };
            floodJob.Schedule().Complete();

            floodMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FindBounds()
        {
            boundsMarker.Begin();

            GetBoundsJob boundsJob = new GetBoundsJob()
            {
                minArray = minArray,
                maxArray = maxArray,

                mask = mask,

                size = component.world.size,
                mul = component.world.mul
            };
            boundsJob.ScheduleParallel(partsCount.Length, 1, default).Complete();

            boundsMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CreateNewWorlds()
        {
            createWorldsMarker.Begin();

            worldList.Clear();

            mainIndex = 0;
            for (int i = 1; i < partsCount.Length; i++)
                mainIndex = math.select(mainIndex, i, partsCount[i] > partsCount[mainIndex]);

            for (int i = 0; i < partsCount.Length; i++)
            {
                if (i == mainIndex)
                {
                    worldList.Add(component);
                    continue;
                }
                worldList.Add(Instantiate(worldComponentPrefab, component.transform.TransformPoint((float3)minArray[i]), component.transform.rotation));

                VoxelWorld newWorld = new VoxelWorld(maxArray[i] - minArray[i] + 1);
                worldList[i].world = newWorld;

                CreateWorldJob createWorldJob = new CreateWorldJob()
                {
                    mainWorld = component.world[VoxelType.Content],
                    mask = mask,
                    newWorld = newWorld[VoxelType.Content],

                    start = minArray[i],
                    mul1 = component.world.mul,
                    mul2 = newWorld.mul,
                    size2 = newWorld.size,

                    label = i + 1
                };
                handle = createWorldJob.Schedule(handle);
            }
            handle.Complete();

            createWorldsMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UpdateWorlds()
        {
            updateWorldMarker.Begin();

            for (int i = 0; i < worldList.Count; i++)
            {
                if (i != mainIndex)
                {
                    worldList[i].rb.velocity = worldList[mainIndex].rb.velocity;
                    worldList[i].rb.angularVelocity = worldList[mainIndex].rb.angularVelocity;
                    worldList[i].UpdateChunks();
                }
                worldList[i].rb.mass = partsCount[i];
                worldList[i].rb.WakeUp();
            }

            if (worldList.Count != 0)
            {
                int3 minBounds = minArray[0], maxBounds = maxArray[0];
                for (int i = 1; i < partsCount.Length; i++)
                    if (i == mainIndex)
                        continue;
                    else
                    {
                        minBounds = math.min(minArray[i], minBounds);
                        maxBounds = math.max(maxArray[i], maxBounds);
                    }
                VoxelWorldBuilder.BuildVoxelWorld(component, minBounds, maxBounds);
            }

            updateWorldMarker.End();
        }

        [BurstCompile]
        private struct FloodFillJob : IJob
        {
            [ReadOnly]
            public NativeArray<byte> voxels;
            public NativeArray<int> mask;
            public NativeList<int3> posStack;
            public NativeList<int> indexStack;

            [WriteOnly]
            public NativeList<int> partsCount;

            [ReadOnly]
            public int3 size, mul;

            public void Execute()
            {
                bool spanUp, spanDown, spanLeft, spanRight;
                int label = 0;

                for (int x = 0, xOff = 0; x < size.x; x++, xOff += mul.x)
                    for (int y = 0, yOff = xOff; y < size.y; y++, yOff += mul.y)
                        for (int z = 0, zOff = yOff; z < size.z; z++, zOff += mul.z)
                            if (voxels[zOff] != 0 && mask[zOff] == 0)
                            {
                                label++;
                                int count = 0;
                                Enqueue(posStack, new int3(x, y, z));
                                Enqueue(indexStack, zOff);

                                while (posStack.Length != 0)
                                {
                                    int3 pos = Dequeue(posStack);
                                    int index = Dequeue(indexStack);

                                    spanUp = spanDown = spanLeft = spanRight = false;

                                    for (; pos.x >= 0 && voxels[index] != 0; pos.x--, index -= mul.x) { }
                                    pos.x++;
                                    index += mul.x;

                                    for (; pos.x < size.x && voxels[index] != 0 && mask[index] == 0; pos.x++, index += mul.x)
                                    {
                                        mask[index] = label;
                                        count++;

                                        if (pos.y > 0)
                                            if (spanUp)
                                                spanUp = voxels[index - mul.y] != 0;
                                            else
                                            {
                                                int finalIndex = index - mul.y;
                                                if (voxels[finalIndex] != 0 && mask[finalIndex] == 0)
                                                {
                                                    Enqueue(posStack, new int3(pos.x, pos.y - 1, pos.z));
                                                    Enqueue(indexStack, finalIndex);
                                                    spanUp = true;
                                                }
                                            }

                                        if (pos.y < size.y - 1)
                                            if (spanDown)
                                                spanDown = voxels[index + mul.y] != 0;
                                            else
                                            {
                                                int finalIndex = index + mul.y;
                                                if (voxels[finalIndex] != 0 && mask[finalIndex] == 0)
                                                {
                                                    Enqueue(posStack, new int3(pos.x, pos.y + 1, pos.z));
                                                    Enqueue(indexStack, finalIndex);
                                                    spanDown = true;
                                                }
                                            }

                                        if (pos.z > 0)
                                            if (spanLeft)
                                                spanLeft = voxels[index - mul.z] != 0;
                                            else
                                            {
                                                int finalIndex = index - mul.z;
                                                if (voxels[finalIndex] != 0 && mask[finalIndex] == 0)
                                                {
                                                    Enqueue(posStack, new int3(pos.x, pos.y, pos.z - 1));
                                                    Enqueue(indexStack, finalIndex);
                                                    spanLeft = true;
                                                }
                                            }

                                        if (pos.z < size.z - 1)
                                            if (spanRight)
                                                spanRight = voxels[index + mul.z] != 0;
                                            else
                                            {
                                                int finalIndex = index + mul.z;
                                                if (voxels[finalIndex] != 0 && mask[finalIndex] == 0)
                                                {
                                                    Enqueue(posStack, new int3(pos.x, pos.y, pos.z + 1));
                                                    Enqueue(indexStack, finalIndex);
                                                    spanRight = true;
                                                }
                                            }
                                    }
                                }
                                partsCount.Add(count);
                            }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private T Dequeue<T>(in NativeList<T> list) where T : struct
            {
                T item = list[list.Length - 1];
                list.RemoveAt(list.Length - 1);
                return item;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void Enqueue<T>(in NativeList<T> list, in T item) where T : struct
            {
                list.Add(item);
            }
        }

        [BurstCompile]
        private struct GetBoundsJob : IJobFor
        {
            [WriteOnly, NativeDisableParallelForRestriction]
            public NativeArray<int3> minArray;
            [WriteOnly, NativeDisableParallelForRestriction]
            public NativeArray<int3> maxArray;

            [ReadOnly]
            public NativeArray<int> mask;

            [ReadOnly]
            public int3 size, mul;

            public void Execute(int index)
            {
                FindMinBounds(index);
                FindMaxBounds(index);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void FindMinBounds(int index)
            {
                int3 min = new int3();
                min.x = FindXMinBound(index + 1);
                min.y = FindYMinBound(index + 1, min.x);
                min.z = FindZMinBound(index + 1, min.x, min.y);
                minArray[index] = min;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void FindMaxBounds(int index)
            {
                int3 max = new int3();
                max.x = FindXMaxBound(index + 1);
                max.y = FindYMaxBound(index + 1, max.x);
                max.z = FindZMaxBound(index + 1, max.x, max.y);
                maxArray[index] = max;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindXMinBound(int index)
            {
                for (int x = 0, xOff = 0; x < size.x; x++, xOff += mul.x)
                    for (int y = 0, yOff = xOff; y < size.y; y++, yOff += mul.y)
                        for (int z = 0, zOff = yOff; z < size.z; z++, zOff += mul.z)
                            if (mask[zOff] == index)
                                return x;
                return size.x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindYMinBound(int index, int startX)
            {
                for (int y = 0, yOff = 0; y < size.y; y++, yOff += mul.y)
                    for (int x = startX, xOff = yOff + x * mul.x; x < size.x; x++, xOff += mul.x)
                        for (int z = 0, zOff = xOff; z < size.z; z++, zOff += mul.z)
                            if (mask[zOff] == index)
                                return y;
                return size.y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindZMinBound(int index, int startX, int startY)
            {
                for (int z = 0, zOff = 0; z < size.z; z++, zOff += mul.z)
                    for (int x = startX, xOff = zOff + x * mul.x; x < size.x; x++, xOff += mul.x)
                        for (int y = startY, yOff = xOff + y * mul.y; y < size.y; y++, yOff += mul.y)
                            if (mask[yOff] == index)
                                return z;
                return size.z;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindXMaxBound(int index)
            {
                for (int x = size.x - 1, xOff = x * mul.x; x >= 0; x--, xOff -= mul.x)
                    for (int y = size.y - 1, yOff = xOff + y * mul.y; y >= 0; y--, yOff -= mul.y)
                        for (int z = size.z - 1, zOff = yOff + z * mul.z; z >= 0; z--, zOff -= mul.z)
                            if (mask[zOff] == index)
                                return x;
                return 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindYMaxBound(int index, int startX)
            {
                for (int y = size.y - 1, yOff = y * mul.y; y >= 0; y--, yOff -= mul.y)
                    for (int x = startX, xOff = yOff + x * mul.x; x >= 0; x--, xOff -= mul.x)
                        for (int z = size.z - 1, zOff = xOff + z * mul.z; z >= 0; z--, zOff -= mul.z)
                            if (mask[zOff] == index)
                                return y;
                return 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindZMaxBound(int index, int startX, int startY)
            {
                for (int z = size.z - 1, zOff = z * mul.z; z >= 0; z--, zOff -= mul.z)
                    for (int x = startX, xOff = zOff + x * mul.x; x >= 0; x--, xOff -= mul.x)
                        for (int y = startY, yOff = xOff + y * mul.y; y >= 0; y--, yOff -= mul.y)
                            if (mask[yOff] == index)
                                return z;
                return 0;
            }
        }

        [BurstCompile]
        private struct CreateWorldJob : IJob
        {
            public NativeArray<byte> mainWorld;
            [ReadOnly]
            public NativeArray<int> mask;
            [WriteOnly]
            public NativeArray<byte> newWorld;

            [ReadOnly]
            public int3 start, mul1, mul2, size2;
            [ReadOnly]
            public int label;

            public void Execute()
            {
                for (int x = 0, from0Off = start.x * mul1.x, to0Off = 0; x < size2.x; x++, from0Off += mul1.x, to0Off += mul2.x)
                    for (int y = 0, from1Off = from0Off + start.y * mul1.y, to1Off = to0Off; y < size2.y; y++, from1Off += mul1.y, to1Off += mul2.y)
                        for (int z = 0, from2Off = from1Off + start.z * mul1.z, to2Off = to1Off; z < size2.z; z++, from2Off += mul1.z, to2Off += mul2.z)
                        {
                            newWorld[to2Off] = (byte)math.select(mainWorld[from2Off], 0, mask[from2Off] != label);
                            mainWorld[from2Off] = (byte)math.select(0, mainWorld[from2Off], mask[from2Off] != label);
                        }
            }
        }
    }
}
