using Game.Voxels;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class FindLineRule : Rule<int>
    {
        /// <summary> Задача, ищущая линию из нужного материала. </summary>
        [BurstCompile]
        private struct FindLineJob : IJob
        {
            [ReadOnly]
            public NativeArray<byte> content;
            [ReadOnly]
            public NativeArray<bool> mask;

            [ReadOnly]
            public int material;

            [ReadOnly]
            public bool posDirection;

            [ReadOnly]
            public int axisSize, axisMul;
            public int axisPos, index;

            [WriteOnly]
            public NativeArray<int> result;

            public void Execute()
            {
                int length = 0;

                if (posDirection)
                    for (; axisPos < axisSize && !mask[index] && (content[index] & 0x0F) == material; length++, index += axisMul, axisPos++) { }
                else
                    for (; axisPos >= 0 && !mask[index] && (content[index] & 0x0F) == material; length++, index -= axisMul, axisPos--) { }

                result[0] = length;
            }
        }

        private FindLineJob job = new FindLineJob();

        private FindBlockRule block;

        private int _totalHealth;
        public override int totalHealth => _totalHealth;

        /// <summary>
        /// Направление поиска линии.
        /// </summary>
        public int direction { get; set; }
        /// <summary>
        /// Стартовая позиция поиска линии.
        /// </summary>
        public override int3 position { get; set; }

        public override event Action<int> executed;

        public FindLineRule(MaterialPredicate predicate)
        {
            block = new FindBlockRule(predicate);
        }

        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask)
        {
            block.position = position;
            if (!block.Execute(world, mask))
                return false;

            int axis = direction % 3;

            job.content = world[VoxelType.Content];
            job.mask = mask;

            job.material = (int)block.material;

            job.posDirection = direction > 2;
            job.axisSize = world.size[axis];
            job.axisMul = world.mul[axis];
            job.axisPos = position[axis];
            job.index = world.GetFlatIndex(position);

            job.result = new NativeArray<int>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            job.Schedule().Complete();
            int3 size = new int3(1, 1, 1);
            size[axis] = job.result[0];
            _totalHealth = ModuleTools.GetHealthSum(world[VoxelType.Health], position, size, world.mul);
            executed?.Invoke(job.result[0]);
            job.result.Dispose();

            return true;
        }
    }
}
