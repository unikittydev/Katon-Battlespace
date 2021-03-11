using Game.Voxels;
using Unity.Collections;

namespace Game.Modules.Creation
{
    /// <summary>
    /// ������� ���������� ������ ������.
    /// </summary>
    public enum RuleGroupType
    {
        /// <summary>
        /// ��� ���������� ������ ���������� ���������� ���� � ������.
        /// </summary>
        All,
        /// <summary>
        /// ��� ���������� ������ ���������� ���������� ���� �� ����� �����.
        /// </summary>
        Any,
        /// <summary>
        /// ��� ���������� ������ ���������� ���������� ������ ���������� �����. ��������� ����������� �� �����.
        /// </summary>
        First,
        /// <summary>
        /// ������ ���������� � ����� ������.
        /// </summary>
        Optional,
        /// <summary>
        /// ��� ���������� ������ ���������� ���������� ������� �������. ��������� ��� ���������� �� �����������.
        /// </summary>
        FirstRequired
    }

    public class RuleGroup : IRule
    {
        /// <summary>
        /// ������ ������ ��� ����� ������.
        /// </summary>
        public IRule[] ruleList { get; }

        private readonly RuleGroupExecute executeMethod;

        /// <summary>
        /// ��������� �������� ������ ������.
        /// </summary>
        public virtual int totalHealth
        {
            get
            {
                int sum = 0;
                for (int i = 0; i < ruleList.Length; i++)
                    sum += ruleList[i].totalHealth;
                return sum;
            }
        }

        public RuleGroup(RuleGroupType type, IRule[] ruleList)
        {
            this.ruleList = ruleList;
            switch (type)
            {
                case RuleGroupType.All:
                    executeMethod = ExecuteAll;
                    break;
                case RuleGroupType.Any:
                    executeMethod = ExecuteAny;
                    break;
                case RuleGroupType.First:
                    executeMethod = ExecuteFirst;
                    break;
                case RuleGroupType.Optional:
                    executeMethod = ExecuteOptional;
                    break;
                case RuleGroupType.FirstRequired:
                    executeMethod = ExecuteFirstRequired;
                    break;
            }
        }

        /// <summary>
        /// ��������� ������ ������ ������ ������.
        /// </summary>
        /// <returns>���������� false, ���� ���� �� ���� ������������ ������� �� ���������. ����� ���������� true.</returns>
        public virtual bool Execute(in VoxelWorld world, in NativeArray<bool> mask)
        {
            bool success = executeMethod(world, mask);
            return success;
        }

        private bool ExecuteAll(in VoxelWorld world, in NativeArray<bool> mask)
        {
            for (int i = 0; i < ruleList.Length; i++)
                if (!ruleList[i].Execute(world, mask))
                    return false;
            return true;
        }

        private bool ExecuteAny(in VoxelWorld world, in NativeArray<bool> mask)
        {
            bool success = false;

            for (int i = 0; i < ruleList.Length; i++)
                success |= ruleList[i].Execute(world, mask);

            return success;
        }

        private bool ExecuteFirst(in VoxelWorld world, in NativeArray<bool> mask)
        {
            for (int i = 0; i < ruleList.Length; i++)
                if (ruleList[i].Execute(world, mask))
                    return true;
            return false;
        }

        private bool ExecuteOptional(in VoxelWorld world, in NativeArray<bool> mask)
        {
            for (int i = 0; i < ruleList.Length; i++)
                ruleList[i].Execute(world, mask);
            return true;
        }

        private bool ExecuteFirstRequired(in VoxelWorld world, in NativeArray<bool> mask)
        {
            if (!ruleList[0].Execute(world, mask))
                return false;
            for (int i = 1; i < ruleList.Length; i++)
                ruleList[i].Execute(world, mask);
            return true;
        }
    }
}
