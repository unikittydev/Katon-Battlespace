using Game.Voxels;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class FindBoxRule : Rule<int3>
    {
        /// <summary> Задача, ищущая коробку из нужного материала. </summary>
        [BurstCompile]
        private struct FindBoxJob : IJob
        {
            [ReadOnly]
            public NativeArray<byte> content;
            [ReadOnly]
            public NativeArray<bool> mask;

            [ReadOnly]
            public int material, startIndex;

            [ReadOnly]
            public int3 size, mul, startPos;

            [WriteOnly]
            public NativeArray<int3> result;

            private int3 currPos, boxSize;

            public void Execute()
            {
                currPos = startPos;
                boxSize.x = FindXSize(++currPos.x, startIndex + mul.x, size.x);

                currPos = startPos;
                boxSize.y = FindYSize(++currPos.y, startIndex + mul.y, startPos.x + boxSize.x, size.y);

                currPos = startPos;
                boxSize.z = FindZSize(++currPos.z, startIndex + mul.z, startPos.x + boxSize.x, startPos.y + boxSize.y, size.z);

                result[0] = boxSize;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool CanMerge(int index)
            {
                return !mask[index] && (content[index] & 0x0F) == material;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindXSize(int startX, int startIndex, int maxSize)
            {
                int xOffset;
                for (currPos.x = startX, xOffset = startIndex; currPos.x < maxSize && CanMerge(xOffset); currPos.x++, xOffset += mul.x) { }
                return currPos.x - startPos.x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindYSize(int startY, int startIndex, int maxSizeX, int maxSizeY)
            {
                int yOffset;
                for (currPos.y = startY, yOffset = startIndex; currPos.y < maxSizeY && FindXSize(startPos.x, yOffset, maxSizeX) == boxSize.x; currPos.y++, yOffset += mul.y) { }
                return currPos.y - startPos.y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindZSize(int startZ, int startIndex, int maxSizeX, int maxSizeY, int maxSizeZ)
            {
                int zOffset;
                for (currPos.z = startZ, zOffset = startIndex; currPos.z < maxSizeZ && FindYSize(startPos.y, zOffset, maxSizeX, maxSizeY) == boxSize.y; currPos.z++, zOffset += mul.z) { }
                return currPos.z - startPos.z;
            }
        }

        private FindBoxJob job = new FindBoxJob();

        private FindBlockRule block;

        private int _totalHealth;
        public override int totalHealth => _totalHealth;

        public override int3 position { get; set; }

        public override event Action<int3> executed;

        public FindBoxRule(MaterialPredicate predicate, Material material)
        {
            block = new FindBlockRule(predicate, material);
        }

        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask)
        {
            block.position = position;
            if (!block.Execute(world, mask))
                return false;

            job.content = world[VoxelType.Content];
            job.mask = mask;

            job.material = (int)block.material;
            job.startIndex = world.GetFlatIndex(position);

            job.size = world.size;
            job.mul = world.mul;
            job.startPos = position;

            job.result = new NativeArray<int3>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            job.Schedule().Complete();
            _totalHealth = ModuleTools.GetHealthSum(world[VoxelType.Health], position, job.result[0], world.mul);
            executed?.Invoke(job.result[0]);
            job.result.Dispose();

            return true;
        }
    }
}
