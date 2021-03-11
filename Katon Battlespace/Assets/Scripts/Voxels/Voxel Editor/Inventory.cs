using System.Collections.Generic;

namespace Game.Voxels.Editor
{
    [System.Serializable]
    public class Inventory
    {
        public List<VoxelItem> items;
        [System.NonSerialized]
        public List<VoxelItemUI> itemSlots;
    }
}
