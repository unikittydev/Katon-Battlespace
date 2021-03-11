using System;

namespace Game.Modules.Creation
{
    /// <summary>
    /// Интерфейс правила или группы правил, который сообщает о выполнении.
    /// </summary>
    public interface IRuleResult
    {
        event Action executed;
    }

    /// <summary>
    /// Интерфейс правила или группы правил, который сообщает результат при выполнении.
    /// </summary>
    /// <typeparam name="T0">Тип возвращаемого результата.</typeparam>
    public interface IRuleResult<T0>
    {
        /// <summary>
        /// Событие выполнения правила.
        /// </summary>
        event Action<T0> executed;
    }

    /// <summary>
    /// Интерфейс правила или группы правил, который сообщает результат при выполнении.
    /// </summary>
    /// <typeparam name="T0">Тип возвращаемого результата.</typeparam>
    public interface IRuleResult<T0, T1>
    {
        /// <summary>
        /// Событие выполнения правила.
        /// </summary>
        event Action<T0, T1> executed;
    }
}
