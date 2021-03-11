using Game.Voxels;
using System;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class EnergyProducerRule : ModuleGroup
    {
        private class CoverRule : RuleGroup, IRuleResult
        {
            public event Action executed;

            public CoverRule(FindBlockRule baseBlock, MaterialPredicate predicate, Material material) : base(RuleGroupType.All, new IRule[6])
            {
                for (int i = 0; i < 6; i++)
                {
                    int index = i;

                    int3 direction = new int3();
                    direction[i % 3] = math.select(1, -1, i > 2);

                    FindBlockRule coverRule = new FindBlockRule(predicate, material);
                    baseBlock.executed += _ => coverRule.position = baseBlock.position + direction;
                    ruleList[index] = coverRule;
                }
            }

            public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask)
            {
                if (base.Execute(world, mask))
                {
                    executed?.Invoke();
                    return true;
                }
                return false;
            }
        }

        private readonly FindBlockRule baseBlock = new FindBlockRule(ModuleTools.TryGetBlock, Material.Glowstone);

        public override int[] health
        {
            get
            {
                _health[1] = ruleList[1].totalHealth / 6;
                return _health;
            }
        }

        public EnergyProducerRule() : base(RuleGroupType.All, new IRule[2])
        {
            ruleList[0] = baseBlock;
            CoverRule cover = new CoverRule(baseBlock, ModuleTools.TryGetBlock, Material.Pipes);
            cover.executed += () =>
            {
                _boundsStart = baseBlock.position - new int3(1, 1, 1);
                _boundsSize = new int3(3, 3, 3);
            };
            ruleList[1] = cover;
        }

        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask, in int3 position)
        {
            baseBlock.position = position;
            return Execute(world, mask);
        }
    }
}
