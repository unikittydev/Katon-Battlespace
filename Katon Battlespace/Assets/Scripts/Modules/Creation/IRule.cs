using Game.Voxels;
using Unity.Collections;

namespace Game.Modules.Creation
{
    /// <summary>
    /// ������� ��������� ��� ������ ��� �� �����.
    /// </summary>
    public interface IRule
    {
        int totalHealth { get; }

        /// <summary>
        /// ���������, ����������� �� ������� �������� ������.
        /// </summary>
        /// <param name="world"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        bool Execute(in VoxelWorld world, in NativeArray<bool> mask);
    }
}
