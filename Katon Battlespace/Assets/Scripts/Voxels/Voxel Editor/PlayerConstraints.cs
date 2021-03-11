using UnityEngine;

namespace Game.Voxels.Editor
{
    public class PlayerConstraints : MonoBehaviour
    {
        [SerializeField]
        private Ship ship;
        [SerializeField]
        private VoxelWorldEditor editor;

        private PlayerController controller;

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            controller.onVoxelPlace.AddListener(OnVoxelPlace);
            controller.onVoxelBreak.AddListener(OnVoxelBreak);
        }

        private void OnDisable()
        {
            controller.onVoxelPlace.RemoveListener(OnVoxelPlace);
            controller.onVoxelBreak.RemoveListener(OnVoxelBreak);
        }

        private void OnVoxelPlace(VoxelPlaceEventArgs args)
        {
            // Макс. масса
            if (editor.voxelCount == editor.maxVoxelCount)
                args.handleCode = VoxelPlaceEventArgs.HandleCode.MaxMassLimit;
            // Макс. размер
            else
                for (int i = 0; i < 3; i++)
                    if ((args.position[i] < 0 || args.position[i] == ship.component.world.size[i]) && ship.component.world.size[i] == editor.maxSize[i])
                    {
                        args.handleCode = VoxelPlaceEventArgs.HandleCode.MaxSizeLimit;
                        break;
                    }
        }

        private void OnVoxelBreak(VoxelBreakEventArgs args)
        {
            // Не ломать последний блок
            if (editor.voxelCount == 1)
                args.handleCode = VoxelBreakEventArgs.HandleCode.LastVoxel;
        }
    }
}
