using Unity.Mathematics;

namespace Game.Voxels.Editor
{
    public class VoxelPlaceEventArgs
    {
        public static readonly string[] cancelWarningMessages = new[]
        {
            "Корабль слишком большой!",
            "Корабль слишком тяжёлый!",
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
            "Нельзя ломать последний воксель!"
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
