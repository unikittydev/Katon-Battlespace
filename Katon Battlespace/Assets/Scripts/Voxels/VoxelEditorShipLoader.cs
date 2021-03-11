using UnityEngine;

namespace Game.Voxels.Editor
{
    public class VoxelEditorShipLoader : MonoBehaviour
    {
        [SerializeField]
        private VoxelWorldComponent component;
        [SerializeField]
        private VoxelWorldEditor worldEditor;

        [SerializeField]
        private ShipListLoader loader;

        public void LoadShip()
        {
            component.OnLoad(GameDataManager.Load(typeof(VoxelWorldComponent), loader.loadShipName));
            worldEditor.OnLoad(GameDataManager.Load(typeof(VoxelWorldEditor), loader.loadShipName));
        }

        public void CreateNewShip()
        {
            component.OnLoad(GameDataManager.LoadDefault(typeof(VoxelWorldComponent)));
            worldEditor.OnLoad(GameDataManager.LoadDefault(typeof(VoxelWorldEditor)));
        }

        public void SaveShip()
        {
            GameDataManager.Save(typeof(VoxelWorldComponent), loader.saveShipName, component.OnSave());
            GameDataManager.Save(typeof(VoxelWorldEditor), loader.saveShipName, worldEditor.OnSave());
        }
    }
}
