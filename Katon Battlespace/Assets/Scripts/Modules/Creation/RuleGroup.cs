using Game.Voxels;
using Unity.Collections;

namespace Game.Modules.Creation
{
    /// <summary>
    /// Условие выполнения группы правил.
    /// </summary>
    public enum RuleGroupType
    {
        /// <summary>
        /// Для выполнения группы необходимо выполнение всех её частей.
        /// </summary>
        All,
        /// <summary>
        /// Для выполнения группы необходимо выполнение хотя бы одной части.
        /// </summary>
        Any,
        /// <summary>
        /// Для выполнения группы необходимо выполнение первой попавшейся части. Остальные выполняться не будут.
        /// </summary>
        First,
        /// <summary>
        /// Группа выполнится в любом случае.
        /// </summary>
        Optional,
        /// <summary>
        /// Для выполнения группы необходимо выполнение первого условия. Остальные для выполнения не обязательны.
        /// </summary>
        FirstRequired
    }

    public class RuleGroup : IRule
    {
        /// <summary>
        /// Список правил или групп правил.
        /// </summary>
        public IRule[] ruleList { get; }

        private readonly RuleGroupExecute executeMethod;

        /// <summary>
        /// Суммарное здоровье группы правил.
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
        /// Выполняет группу правил поиска модуля.
        /// </summary>
        /// <returns>Возвращает false, если хотя бы одно обязательное условие не выполнено. Иначе возвращает true.</returns>
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
