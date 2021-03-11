using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Game.Voxels
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VoxelVertex
    {
        public float3 pos;
        public float3 normal;
        public half4 tangent;
        public uint color;
        public half2 uv;
        public half2 uv2;
    }
}
