using Game.Voxels;
using UnityEngine;

namespace Game
{
    public class SingleShipLoader : MonoBehaviour
    {
        [SerializeField]
        private VoxelWorldComponent ship;

        [SerializeField]
        private VoxelWorldComponent[] targets;

        public static string shipName { get; set; }

        public void Awake()
        {
            ship.OnLoad(GameDataManager.Load(typeof(VoxelWorldComponent), shipName));
            for (int i = 0; i < targets.Length; i++)
            {
                Vector3 pos = targets[i].transform.position;
                targets[i].OnLoad(GameDataManager.Load(typeof(VoxelWorldComponent), shipName));
                targets[i].transform.position = pos;
            }
        }
    }
}

