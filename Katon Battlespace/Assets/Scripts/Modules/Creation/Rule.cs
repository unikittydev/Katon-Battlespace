using Game.Voxels;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    /// <summary>
    /// Базовый класс для правил поиска модулей.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого результата.</typeparam>
    public abstract class Rule<T> : IRule, IRuleResult<T>
    {
        /// <summary>
        /// Суммарное здоровье правила.
        /// </summary>
        public abstract int totalHealth { get; }

        /// <summary>
        /// Позиция, от которой нужно проверять правило.
        /// </summary>
        public abstract int3 position { get; set; }

        /// <summary>
        /// Событие выполнения правила.
        /// </summary>
        public abstract event System.Action<T> executed;

        /// <summary>
        /// Выполняет правило поиска модулей.
        /// </summary>
        public abstract bool Execute(in VoxelWorld world, in NativeArray<bool> mask);
    }
}
