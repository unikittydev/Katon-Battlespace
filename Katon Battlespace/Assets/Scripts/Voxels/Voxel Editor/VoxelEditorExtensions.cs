using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game.Voxels.Editor
{
    public static class VoxelEditorTools
    {
        [BurstCompile]
        private struct FillAreaJob : IJob
        {
            [WriteOnly]
            public NativeArray<byte> voxels;

            public byte voxel;

            [ReadOnly]
            public int3 start, end, mul;

            public void Execute()
            {
                for (int x = start.x, xOff = start.x * mul.x; x < end.x; x++, xOff += mul.x)
                    for (int y = start.y, yOff = xOff + start.y * mul.y; y < end.y; y++, yOff += mul.y)
                        for (int z = start.z, zOff = yOff + start.z * mul.z; z < end.z; z++, zOff += mul.z)
                            voxels[zOff] = voxel;
            }
        }

        public static void FillArea(in VoxelWorld world, int3 start, int3 end, byte voxel, byte health)
        {
            FillAreaJob voxelJob = new FillAreaJob()
            {
                voxels = world[VoxelType.Content],
                voxel = voxel,

                start = start,
                end = end,
                mul = world.mul
            };
            JobHandle voxelHandle = voxelJob.Schedule();

            FillAreaJob healthJob = new FillAreaJob()
            {
                voxels = world[VoxelType.Health],
                voxel = health,

                start = start,
                end = end,
                mul = world.mul
            };
            JobHandle healthHandle = healthJob.Schedule();

            JobHandle.CompleteAll(ref voxelHandle, ref healthHandle);
        }

        [BurstCompile]
        private struct OutliteAreaJob : IJob
        {
            [WriteOnly]
            public NativeArray<byte> voxels;

            public byte voxel;

            [ReadOnly]
            public int3 start, end, mul;

            public void Execute()
            {
                for (int y = start.y, yOff = start.x * mul.x + y * mul.y; y < end.y; y++, yOff += mul.y)
                    for (int z = start.z, zOff = yOff + z * mul.z; z < end.z; z++, zOff += mul.z)
                        voxels[zOff] = voxel;
                for (int x = start.x + 1, xOff = x * mul.x; x < end.x - 1; x++, xOff += mul.x)
                {
                    int index, y, z;
                    for (y = start.y, index = xOff + y * mul.y + start.z * mul.z; y < end.y; y++, index += mul.y)
                        voxels[index] = voxel;
                    for (y = start.y, index = xOff + y * mul.y + (end.z - start.z - 1) * mul.z; y < end.y; y++, index += mul.y)
                        voxels[index] = voxel;
                    for (z = start.z, index = xOff + start.y * mul.y + z * mul.z; z < end.z; z++, index += mul.z)
                        voxels[index] = voxel;
                    for (z = start.z, index = xOff + (end.y - start.y - 1) * mul.y + z * mul.z; z < end.z; z++, index += mul.z)
                        voxels[index] = voxel;
                }
                for (int y = start.y, yOff = (end.x - 1) * mul.x + start.y * mul.y; y < end.y; y++, yOff += mul.y)
                    for (int z = start.z, zOff = yOff + start.z * mul.z; z < end.z; z++, zOff += mul.z)
                        voxels[zOff] = voxel;
            }
        }

        public static void OutliteArea(in VoxelWorld world, int3 start, int3 end, byte voxel, byte health)
        {
            OutliteAreaJob voxelJob = new OutliteAreaJob()
            {
                voxels = world[VoxelType.Content],
                voxel = voxel,

                start = start,
                end = end,
                mul = world.mul
            };
            JobHandle voxelHandle = voxelJob.Schedule();

            OutliteAreaJob healthJob = new OutliteAreaJob()
            {
                voxels = world[VoxelType.Health],
                voxel = health,

                start = start,
                end = end,
                mul = world.mul
            };
            JobHandle healthHandle = healthJob.Schedule();

            JobHandle.CompleteAll(ref voxelHandle, ref healthHandle);
        }

        [BurstCompile]
        private struct TrimWorldJob : IJob
        {
            [ReadOnly]
            public NativeArray<byte> voxels;

            [ReadOnly]
            public int3 size, mul;
            public NativeArray<int3> result;

            public void Execute()
            {
                for (int dir = 0; dir < 3; dir++)
                {
                    int ax1 = (dir + 1) % 3;
                    int ax2 = (dir + 2) % 3;
                    TrimPosAxis(dir, ax1, ax2);
                    TrimNegAxis(dir, ax1, ax2);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void TrimPosAxis(int dir, int ax1, int ax2)
            {
                int3 min;
                for (int cp0 = 0, cp0Off = 0; cp0 < size[dir]; cp0++, cp0Off += mul[dir])
                {
                    for (int cp1 = 0, cp1Off = cp0Off; cp1 < size[ax1]; cp1++, cp1Off += mul[ax1])
                        for (int cp2 = 0, cp2Off = cp1Off; cp2 < size[ax2]; cp2++, cp2Off += mul[ax2])
                            if (voxels[cp2Off] != 0)
                                return;
                    min = result[0];
                    min[dir] = cp0 + 1;
                    result[0] = min;
                }
                min = result[0];
                min[dir] = 0;
                result[0] = min;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void TrimNegAxis(int dir, int ax1, int ax2)
            {
                int3 max;
                for (int cp0 = size[dir] - 1, cp0Off = cp0 * mul[dir]; cp0 >= 0; cp0--, cp0Off -= mul[dir])
                {
                    for (int cp1 = size[ax1] - 1, cp1Off = cp0Off + cp1 * mul[ax1]; cp1 >= 0; cp1--, cp1Off -= mul[ax1])
                        for (int cp2 = size[ax2] - 1, cp2Off = cp1Off + cp2 * mul[ax2]; cp2 >= 0; cp2--, cp2Off -= mul[ax2])
                            if (voxels[cp2Off] != 0)
                                return;
                    max = result[1];
                    max[dir] = size[dir] - cp0;
                    result[1] = max;
                }
                max = result[1];
                max[dir] = 0;
                result[1] = max;
            }
        }

        public static NativeArray<int3> TrimWorld(in VoxelWorld world)
        {
            TrimWorldJob job = new TrimWorldJob()
            {
                voxels = world[VoxelType.Content],

                size = world.size,
                mul = world.mul,

                result = new NativeArray<int3>(2, Allocator.TempJob)
            };
            job.Schedule().Complete();

            return job.result;
        }

        [BurstCompile]
        private struct GetAreaJob : IJob
        {
            [ReadOnly]
            public NativeArray<byte> voxels;
            [WriteOnly]
            public NativeArray<byte> area;

            [ReadOnly]
            public int3 start, fromMul, toMul, areaSize;

            public void Execute()
            {
                for (int x = 0, from0Off = start.x * fromMul.x, to0Off = 0; x < areaSize.x; x++, from0Off += fromMul.x, to0Off += toMul.x)
                    for (int y = 0, from1Off = from0Off + start.y * fromMul.y, to1Off = to0Off; y < areaSize.y; y++, from1Off += fromMul.y, to1Off += toMul.y)
                        for (int z = 0, from2Off = from1Off + start.z * fromMul.z, to2Off = to1Off; z < areaSize.z; z++, from2Off += fromMul.z, to2Off += toMul.z)
                            area[to2Off] = voxels[from2Off];
            }
        }

        public static VoxelWorld CopyArea(in VoxelWorld world, int3 start, int3 end)
        {
            VoxelWorld areaWorld = new VoxelWorld(end - start, Allocator.TempJob);

            NativeArray<JobHandle> handles = new NativeArray<JobHandle>(VoxelWorld.voxelTypes, Allocator.Temp);

            for (int i = 0; i < VoxelWorld.voxelTypes; i++)
            {
                GetAreaJob job = new GetAreaJob()
                {
                    voxels = world[(VoxelType)i],
                    area = areaWorld[(VoxelType)i],
                    start = start,
                    fromMul = world.mul,
                    toMul = areaWorld.mul,
                    areaSize = areaWorld.size
                };
                handles[i] = job.Schedule();
            }
            JobHandle.CompleteAll(handles);
            handles.Dispose();

            return areaWorld;
        }

        [BurstCompile]
        private struct SetAreaJob : IJob
        {
            [WriteOnly]
            public NativeArray<byte> voxels;
            [ReadOnly]
            public NativeArray<byte> area;

            [ReadOnly]
            public int3 fromStart, toStart, fromMul, toMul, maxSize;

            public void Execute()
            {
                for (int x = 0, from0Off = fromStart.x * fromMul.x, to0Off = toStart.x * toMul.x; x < maxSize.x; x++, from0Off += fromMul.x, to0Off += toMul.x)
                    for (int y = 0, from1Off = from0Off + fromStart.y * fromMul.y, to1Off = to0Off + toStart.y * toMul.y; y < maxSize.y; y++, from1Off += fromMul.y, to1Off += toMul.y)
                        for (int z = 0, from2Off = from1Off + fromStart.z * fromMul.z, to2Off = to1Off + toStart.z * toMul.z; z < maxSize.z; z++, from2Off += fromMul.z, to2Off += toMul.z)
                            voxels[from2Off] = area[to2Off];
            }
        }

        public static void PasteArea(in VoxelWorld world, in VoxelWorld area, int3 dest, int3 areaStart, int3 areaEnd)
        {
            NativeArray<JobHandle> handles = new NativeArray<JobHandle>(VoxelWorld.voxelTypes, Allocator.Temp);
            for (int i = 0; i < VoxelWorld.voxelTypes; i++)
            {
                SetAreaJob job = new SetAreaJob()
                {
                    voxels = world[(VoxelType)i],
                    area = area[(VoxelType)i],
                    fromStart = math.max(dest, int3.zero),
                    toStart = math.max(areaStart, int3.zero),
                    fromMul = world.mul,
                    toMul = area.mul,
                    maxSize = math.min(math.min(area.size, world.size), areaEnd)
                };
                handles[i] = job.Schedule();
            }

            JobHandle.CompleteAll(handles);
            handles.Dispose();
        }

        public static void CloneArea(in VoxelWorld world, int3 start, int3 end, int3 dest)
        {
            VoxelWorld area = CopyArea(world, start, end);
            PasteArea(world, area, dest, start, end);
            area.Dispose();
        }

        public static void MoveArea(in VoxelWorld world, int3 start, int3 end, int3 dest)
        {
            FillArea(world, start, end, 0, 0);
            VoxelWorld area = CopyArea(world, start, end);
            PasteArea(world, area, dest, start, end);
            area.Dispose();
        }
    }
}
