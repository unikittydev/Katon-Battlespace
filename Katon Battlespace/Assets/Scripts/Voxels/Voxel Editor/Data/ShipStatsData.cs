using Unity.Mathematics;

namespace Game.Voxels.Editor.Data
{
    [System.Serializable]
    public class ShipStatsData
    {
        public string name = "autosave";

        public int maxVoxelCount = 150;
        public int voxelCount = 1;

        public int totalHealth = 10;

        public int3 maxSize = new int3(16, 16, 16);
    }
}
