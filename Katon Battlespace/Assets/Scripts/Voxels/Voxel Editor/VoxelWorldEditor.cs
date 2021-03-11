using Game.Modules;
using Game.Modules.Creation;
using Game.Voxels.Editor.Data;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Voxels.Editor
{
    [RequireComponent(typeof(VoxelWorldComponent))]
    public class VoxelWorldEditor : MonoBehaviour, IGameData
    {
        private VoxelWorldComponent _component;
        public VoxelWorldComponent component => _component;

        private VoxelWorldEditorUI _editorUI;
        public VoxelWorldEditorUI editorUI => _editorUI;

        private Ship _ship;
        public Ship ship => _ship;

        private int _maxVoxelCount;
        public int maxVoxelCount => _maxVoxelCount;

        private int _voxelCount;
        public int voxelCount => _voxelCount;

        private int _totalHealth;
        public int totalHealth => _totalHealth;

        private int3 _maxSize;
        public int3 maxSize => _maxSize;

        public int3 size => component.world.size;

        private void Start()
        {
            _component = GetComponent<VoxelWorldComponent>();
            _editorUI = GetComponent<VoxelWorldEditorUI>();
            _ship = GetComponent<Ship>();
        }

        public void OnLoad(string json)
        {
            ShipStatsData data = JsonUtility.FromJson<ShipStatsData>(json);
            _maxVoxelCount = data.maxVoxelCount;
            _voxelCount = data.voxelCount;
            _totalHealth = data.totalHealth;
            _maxSize = data.maxSize;
            transform.position = Vector3.zero;
        }

        public string OnSave()
        {
            return JsonUtility.ToJson(new ShipStatsData()
            {
                name = name,
                maxVoxelCount = maxVoxelCount,
                voxelCount = voxelCount,
                totalHealth = totalHealth,
                maxSize = maxSize
            });
        }

        public void PlaceVoxel(in int3 pos, byte content, byte health)
        {
            _voxelCount++;
            _totalHealth += health;

            component.rb.mass++;
            component.world.SetVoxel(pos, content, health);
        }

        public void BreakVoxel(in int3 pos)
        {
            int index = component.world.GetFlatIndex(pos);

            _voxelCount--;
            _totalHealth -= component.world[VoxelType.Health][index];

            component.rb.mass--;
            component.world.SetVoxel(index, 0, 0);
        }

        public void FindModules()
        {
            ship.modules.ResizeMask(component.world.size);
            ship.modules.Update();
        }
    }
}
