using Game.Voxels.Editor.Data;
using Unity.Mathematics;

namespace Game.Voxels.Data
{
    [System.Serializable]
    public class VoxelWorldData
    {
        public int3 size = new int3(1, 1, 1);
        public byte[] content = new byte[] { (5) << 4 | 0 };
        public byte[] health = new byte[] { 10 };

        public float3 position;

        public ShipStatsData stats = new ShipStatsData();
    }
}
