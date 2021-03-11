using Game.Voxels;
using System;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class FindBlockRule : Rule<Material>
    {
        private int _totalHealth;
        public override int totalHealth => _totalHealth;

        public override int3 position { get; set; }

        public override event Action<Material> executed;

        private Material _material;
        public Material material => _material;

        private readonly MaterialPredicate materialPredicate;

        public FindBlockRule(MaterialPredicate predicate)
        {
            materialPredicate = predicate;
        }

        public FindBlockRule(MaterialPredicate predicate, Material material)
        {
            _material = material;
            materialPredicate = predicate;
        }

        /// <summary>
        /// »щет воксель по нужному шаблону.
        /// </summary>
        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask)
        {
            int index = world.GetFlatIndex(position);
            bool success = world.ContainsPosition(position) && materialPredicate(world, mask, index, ref _material);
            if (success)
            {
                _totalHealth = world[VoxelType.Health][index];
                executed?.Invoke(material);
                return true;
            }
            _totalHealth = 0;

            return false;
        }
    }
}
