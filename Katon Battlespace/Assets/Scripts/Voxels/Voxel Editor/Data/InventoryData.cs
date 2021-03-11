using System.Collections.Generic;

namespace Game.Voxels.Editor.Data
{
    [System.Serializable]
    public class InventoryData
    {
        public Inventory inventory = new Inventory()
        {
            items = new List<VoxelItem>()
            {
                new VoxelItem() { content = (3 << 4) | 0, health = 10, amount = 15 },
                new VoxelItem() { content = (4 << 4) | 0, health = 15, amount = 20 },
                new VoxelItem() { content = (10 << 4) | 0, health = 10, amount = 20 },
            }
        };

        public Inventory hotbar = new Inventory()
        {
            items = new List<VoxelItem>()
            {
                new VoxelItem() { content = (0 << 4) | 1, health = 10, amount = 1 },
                new VoxelItem() { content = (1 << 4) | 10, health = 15, amount = 10 },
                new VoxelItem() { content = (2 << 4) | 12, health = 25, amount = 12 },
                new VoxelItem() { content = (3 << 4) | 2, health = 25, amount = 2 },
                new VoxelItem() { content = (4 << 4) | 6, health = 25, amount = 6 },
                new VoxelItem() { content = (5 << 4) | 9, health = 25, amount = 9 },
                new VoxelItem() { content = (6 << 4) | 7, health = 25, amount = 7 },
                new VoxelItem() { content = (7 << 4) | 8, health = 25, amount = 8 },
            }
        };

        public int selectedHotbarIndex = 0;
        public int selectedInventoryIndex = -1;
    }
}
