using Game.Voxels;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public class ModuleGroup : RuleGroup
    {
        protected int3 _boundsStart;
        public int3 boundsStart => _boundsStart;

        protected int3 _boundsSize;
        public int3 boundsSize => _boundsSize;

        protected int[] _health;
        public virtual int[] health
        {
            get
            {
                for (int i = 0; i < ruleList.Length; i++)
                    _health[i] = ruleList[i].totalHealth;
                return _health;
            }
        }

        public ModuleGroup(RuleGroupType type, IRule[] ruleList) : base(type, ruleList)
        {
            _health = new int[ruleList.Length];
        }

        public virtual bool Execute(in VoxelWorld world, in NativeArray<bool> mask, in int3 position)
        {
            throw new System.NotImplementedException();
        }
    }
}
