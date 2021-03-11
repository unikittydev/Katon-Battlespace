using Game.Voxels;
using Unity.Collections;

namespace Game.Modules.Creation
{
    /// <summary>
    /// Базовый интерфейс для правил или их групп.
    /// </summary>
    public interface IRule
    {
        int totalHealth { get; }

        /// <summary>
        /// Проверяет, выполняется ли условие создания модуля.
        /// </summary>
        /// <param name="world"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        bool Execute(in VoxelWorld world, in NativeArray<bool> mask);
    }
}
