using Game.Voxels;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class WeaponRule : DirectedModuleGroup
    {
        public Material material => typeBlock.material;

        private FindBlockRule baseBlock = new FindBlockRule(ModuleTools.TryGetBlock, Material.Computer);
        private FindBlockRule typeBlock = new FindBlockRule(ModuleTools.TryGetBlock, Material.Pipes);
        private FindLineRule line = new FindLineRule(ModuleTools.IsSolidBlock);

        public WeaponRule() : base(RuleGroupType.All, new IRule[3])
        {
            ruleList[0] = baseBlock;
            ruleList[1] = typeBlock;
            ruleList[2] = line;

            baseBlock.executed += _ => typeBlock.position = baseBlock.position + dir;
            typeBlock.executed += _ =>
            {
                line.direction = i;
                line.position = typeBlock.position + dir;
            };
            line.executed += lineLength =>
            {
                _boundsSize = new int3(1, 1, 1);
                _boundsSize[i % 3] += lineLength + 1;
                _boundsStart = baseBlock.position - math.select(_boundsSize - 1, 0, i > 2);
            };
        }

        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask, in int3 position)
        {
            baseBlock.position = position;
            return base.Execute(world, mask, position);
        }
    }
}
