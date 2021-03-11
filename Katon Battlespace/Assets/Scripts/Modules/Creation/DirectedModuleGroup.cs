using Game.Voxels;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class DirectedModuleGroup : ModuleGroup
    {
        public int i { get; protected set; }
        public int3 dir { get; protected set; }

        public DirectedModuleGroup(RuleGroupType type, IRule[] ruleList) : base(type, ruleList)
        {

        }

        public override bool Execute(in VoxelWorld world, in NativeArray<bool> mask, in int3 position)
        {
            for (i = 0; i < 6; i++)
            {
                int3 dir = new int3();
                dir[i % 3] = math.select(-1, 1, i > 2);
                this.dir = dir;

                if (Execute(world, mask))
                    return true;
            }

            return false;
        }
    }
}
