using Game.Voxels;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class FuelStorageRule : ModuleGroup
    {
        private readonly FindBoxRule findBox = new FindBoxRule(ModuleTools.TryGetBlock, Material.FuelTank);

        public FuelStorageRule() : base(RuleGroupType.All, new IRule[1])
        {
            ruleList[0] = findBox;
            findBox.executed += boxSize =>
            {
                _boundsStart = findBox.position;
                _boundsSize = boxSize;
            };
        }

        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask, in int3 position)
        {
            findBox.position = position;
            return Execute(world, mask);
        }
    }
}
