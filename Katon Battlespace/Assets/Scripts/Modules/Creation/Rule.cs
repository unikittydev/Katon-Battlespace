using Game.Voxels;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    /// <summary>
    /// ������� ����� ��� ������ ������ �������.
    /// </summary>
    /// <typeparam name="T">��� ������������� ����������.</typeparam>
    public abstract class Rule<T> : IRule, IRuleResult<T>
    {
        /// <summary>
        /// ��������� �������� �������.
        /// </summary>
        public abstract int totalHealth { get; }

        /// <summary>
        /// �������, �� ������� ����� ��������� �������.
        /// </summary>
        public abstract int3 position { get; set; }

        /// <summary>
        /// ������� ���������� �������.
        /// </summary>
        public abstract event System.Action<T> executed;

        /// <summary>
        /// ��������� ������� ������ �������.
        /// </summary>
        public abstract bool Execute(in VoxelWorld world, in NativeArray<bool> mask);
    }
}
