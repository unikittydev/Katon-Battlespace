using Unity.Mathematics;

namespace Game.Voxels.Editor.Data
{
    [System.Serializable]
    public class PlayerControllerData
    {
        public float3 position = new float3(.5f, 1f, -5f);
        public float3 playerRotation;
        public float3 cameraRotation;
    }
}
