using Game.Voxels;
using UnityEngine;

namespace Game
{
    public class SingleShipLoader : MonoBehaviour
    {
        [SerializeField]
        private VoxelWorldComponent ship;

        public static string shipName { get; set; }

        public void Awake()
        {
            ship.OnLoad(GameDataManager.Load(typeof(VoxelWorldComponent), shipName));
        }
    }
}

