using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering;

namespace Game.Voxels.Editor
{
    public class VoxelRenderer2D : MonoBehaviour
    {
        [SerializeField]
        private Camera voxelCamera;

        [SerializeField]
        private VoxelWorldComponent virtualWorld;

        private void Awake()
        {
            virtualWorld.world = new VoxelWorld(new int3(1, 1, 1), Unity.Collections.Allocator.Persistent);
            virtualWorld.UpdateChunks();
            virtualWorld.transform.GetChild(0).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Voxel UI");
            virtualWorld.transform.GetChild(1).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Voxel UI");

            voxelCamera.enabled = false;
        }

        public void CreateVoxelTexture(RawImage image)
        {
            int textureSize = voxelCamera.targetTexture.width;
            Texture2D texture = new Texture2D(textureSize, textureSize, voxelCamera.targetTexture.graphicsFormat, TextureCreationFlags.None);
            image.texture = texture;

            image.color = new Color(1f, 1f, 1f, 1f);
        }

        public void UpdateVoxelTexture(VoxelItem item, RawImage image)
        {
            if (item.content == 0)
                return;

            virtualWorld.world.SetContent(0, item.content);
            VoxelWorldBuilder.BuildVoxelWorld(virtualWorld, int3.zero, int3.zero);

            RenderTexture.active = voxelCamera.targetTexture;
            voxelCamera.Render();

            Texture2D texture = image.texture as Texture2D;

            texture.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0, false);
            texture.Apply();

            RenderTexture.active = null;
        }
    }
}
