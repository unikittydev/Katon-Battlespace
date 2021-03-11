using Game.Voxels;
using System;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class GyrodineRule : ModuleGroup
    {
        private class GyrodineAxisRule : RuleGroup, IRuleResult
        {
            public FindBlockRule[] requiredAxis = new FindBlockRule[4];
            public FindBlockRule[] optionalAxis = new FindBlockRule[4];

            public event Action executed;

            public GyrodineAxisRule(int axis, FindBlockRule baseBlock) : base(RuleGroupType.FirstRequired, new IRule[2])
            {
                int axis1 = (axis + 1) % 3;
                int axis2 = (axis + 2) % 3;

                for (int x = 0; x < 2; x++)
                    for (int y = 0; y < 2; y++)
                    {
                        int index = x << 1 | y;

                        requiredAxis[index] = new FindBlockRule(ModuleTools.TryGetBlock, Material.Metall);
                        optionalAxis[index] = new FindBlockRule(ModuleTools.TryGetBlock, Material.Girder);

                        int3 reqPos = new int3(), optPos = new int3();
                        reqPos[(axis1 + x) % 3] = optPos[axis2] = math.select(-1, 1, y == 0);
                        optPos[axis1] = math.select(-1, 1, x == 0);

                        baseBlock.executed += _ =>
                        {
                            requiredAxis[index].position = baseBlock.position + reqPos;
                            optionalAxis[index].position = baseBlock.position + optPos;
                        };
                    }

                ruleList[0] = new RuleGroup(RuleGroupType.All, requiredAxis);
                ruleList[1] = new RuleGroup(RuleGroupType.All, optionalAxis);
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

        public override int[] health => _health;

        public FindBlockRule baseBlock = new FindBlockRule(ModuleTools.TryGetBlock, Material.Coil);

        public GyrodineRule() : base(RuleGroupType.FirstRequired, new IRule[4])
        {
            ruleList[0] = baseBlock;
            for (int i = 0; i < 3; i++)
            {
                int axis1 = (i + 1) % 3, axis2 = (i + 2) % 3;

                GyrodineAxisRule axisRule = new GyrodineAxisRule(i, baseBlock);
                axisRule.executed += () =>
                {
                    _boundsSize[axis1] = 3;
                    _boundsSize[axis2] = 3;
                    _boundsStart = baseBlock.position - math.select(int3.zero, new int3(1, 1, 1), _boundsSize == new int3(3, 3, 3));
                };
                ruleList[i + 1] = axisRule;
            }
        }

        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask, in int3 position)
        {
            _boundsStart = position;
            _boundsSize = new int3(1, 1, 1);
            baseBlock.position = position;

            if (!baseBlock.Execute(world, mask))
                return false;
            for (int i = 1; i < 4; i++)
                health[i] = math.select(0, ruleList[i].totalHealth, ruleList[i].Execute(world, mask));

            return true;
        }
    }
}
