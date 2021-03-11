using Game.Voxels.Editor;
using UnityEngine;

namespace Game
{
    public class GameDataLoader : MonoBehaviour
    {
        [SerializeField]
        private Component[] components;

        private void Awake()
        {
            for (int i = 0; i < components.Length; i++)
                (components[i] as IGameData).OnLoad(GameDataManager.Load(components[i].GetType()));
        }

        public void OnSceneExit()
        {
            OnExit(true);
        }

        private void OnApplicationQuit()
        {
            OnExit(false);
        }

        private void OnExit(bool save)
        {
            if (!save)
                return;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is VoxelWorldEditor)
                    components[i].name = "autosave";
                GameDataManager.Save(components[i].GetType(), (components[i] as IGameData).OnSave());
            }
        }
    }
}
