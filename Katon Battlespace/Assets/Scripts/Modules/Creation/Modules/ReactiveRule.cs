using Game.Voxels;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class ReactiveRule : DirectedModuleGroup
    {
        private FindBlockRule baseBlock = new FindBlockRule(ModuleTools.TryGetBlock, Material.Pipes);
        private FindLineRule line = new FindLineRule(ModuleTools.IsSolidBlock);
        private FindBlockRule glowstone = new FindBlockRule(ModuleTools.TryGetBlock, Material.Glowstone);

        public ReactiveRule() : base(RuleGroupType.All, new IRule[3])
        {
            ruleList[0] = baseBlock;
            ruleList[1] = line;
            ruleList[2] = glowstone;

            baseBlock.executed += _ =>
            {
                line.direction = i;
                line.position = baseBlock.position + dir;
            };
            line.executed += lineLength => glowstone.position = baseBlock.position + dir * (lineLength + 1);
            glowstone.executed += _ =>
            {
                _boundsStart = math.min(baseBlock.position, glowstone.position);
                _boundsSize = math.max(baseBlock.position, glowstone.position) - _boundsStart + new int3(1, 1, 1);
            };
        }

        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask, in int3 position)
        {
            baseBlock.position = position;
            return base.Execute(world, mask, position);
        }
    }
}
