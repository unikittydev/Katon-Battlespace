using Game.Modules;
using Game.Voxels;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody), typeof(VoxelWorldComponent))]
    public class Ship : MonoBehaviour
    {
        public VoxelWorldComponent component { get; private set; }

        public ShipController controller { get; private set; }

        public ModuleContainer modules { get; private set; }

        [SerializeField]
        private bool showModuleInfo = false;

        private void Awake()
        {
            component = GetComponent<VoxelWorldComponent>();
            controller = GetComponent<ShipController>();

            modules = new ModuleContainer(Module.moduleTypes, this);
            modules.ResizeMask(component.world.size);
        }

        private void Start()
        {
            modules.Update();
        }

        private void OnDestroy()
        {
            modules.mask.Dispose();
            modules.Clear();
        }

        private void OnGUI()
        {
            if (showModuleInfo)
            {
                string info = "";
                for (int t = 0; t < Module.moduleTypes.Length; t++)
                    for (int i = 0; i < modules.Count(Module.moduleTypes[t]); i++)
                        info += modules.Get(Module.moduleTypes[t], i).ToString() + System.Environment.NewLine;
                GUILayout.Label(info);
            }
        }
    }
}
