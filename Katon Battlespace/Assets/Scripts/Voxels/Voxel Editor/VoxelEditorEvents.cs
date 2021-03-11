using Unity.Mathematics;

namespace Game.Voxels.Editor
{
    public class VoxelPlaceEventArgs
    {
        public static readonly string[] cancelWarningMessages = new[]
        {
            "������� ������� �������!",
            "������� ������� ������!",
            ""
        };

        public enum HandleCode : byte
        {
            None,
            MaxSizeLimit,
            MaxMassLimit,
            EmptyVoxelPlace
        }

        public VoxelItem selectedItem;
        public int3 position;
        public HandleCode handleCode;
    }

    public class VoxelBreakEventArgs
    {
        public static readonly string[] cancelWarningMessages = new[]
        {
            "������ ������ ��������� �������!"
        };

        public enum HandleCode : byte
        {
            None,
            LastVoxel,
        }

        public int3 position;
        public HandleCode handleCode;
    }
}
