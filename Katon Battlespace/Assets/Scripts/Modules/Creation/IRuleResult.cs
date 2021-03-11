using System;

namespace Game.Modules.Creation
{
    /// <summary>
    /// ��������� ������� ��� ������ ������, ������� �������� � ����������.
    /// </summary>
    public interface IRuleResult
    {
        event Action executed;
    }

    /// <summary>
    /// ��������� ������� ��� ������ ������, ������� �������� ��������� ��� ����������.
    /// </summary>
    /// <typeparam name="T0">��� ������������� ����������.</typeparam>
    public interface IRuleResult<T0>
    {
        /// <summary>
        /// ������� ���������� �������.
        /// </summary>
        event Action<T0> executed;
    }

    /// <summary>
    /// ��������� ������� ��� ������ ������, ������� �������� ��������� ��� ����������.
    /// </summary>
    /// <typeparam name="T0">��� ������������� ����������.</typeparam>
    public interface IRuleResult<T0, T1>
    {
        /// <summary>
        /// ������� ���������� �������.
        /// </summary>
        event Action<T0, T1> executed;
    }
}
