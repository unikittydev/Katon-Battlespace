using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Voxels
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshChunk : MonoBehaviour
    {
        public const int materialCount = 3;

        [SerializeField]
        private MeshFilter _meshFilter = null;
        public MeshFilter meshFilter => _meshFilter;

        private const MeshUpdateFlags flags = MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontValidateIndices;

        private static readonly VertexAttributeDescriptor[] descriptors = new[]
        {
            new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float16, 4),
            new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4),
            new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 2),
            new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float16, 2)
        };

        private static readonly ProfilerMarker updateMesh = new ProfilerMarker("MeshChunk.UpdateMesh");

        private NativeArray<byte>[] masks;

        private void Awake()
        {
            meshFilter.mesh = new Mesh();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < masks.Length; i++)
                masks[i].Dispose();
        }

        public void CreateMasks(int chunkArea)
        {
            masks = new NativeArray<byte>[materialCount];
            for (int i = 0; i < materialCount; i++)
                masks[i] = new NativeArray<byte>(chunkArea, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        }

        public NativeArray<byte> GetClearMask(int id)
        {
            unsafe
            {
                UnsafeUtility.MemClear(masks[id].GetUnsafePtr(), masks[id].Length);
            }
            return masks[id];
        }

        public void UpdateMesh(VoxelWorldComponent component, List<NativeList<ushort>> allIndices, List<NativeList<VoxelVertex>> allVertices, Vector3 chunkPos, int startIndex)
        {
            updateMesh.Begin();

            Mesh mesh = meshFilter.mesh;
            mesh.Clear();
            mesh.MarkDynamic();
            mesh.subMeshCount = materialCount;

            int totalVertices = 0;
            for (int i = startIndex; i < startIndex + materialCount; totalVertices += allVertices[i].Length, i++) { }

            mesh.SetVertexBufferParams(totalVertices, descriptors);

            totalVertices = 0;
            for (int i = startIndex; i < startIndex + materialCount; totalVertices += allVertices[i].Length, i++)
                mesh.SetVertexBufferData<VoxelVertex>(allVertices[i], 0, totalVertices, allVertices[i].Length, 0, flags);

            int totalIndices = 0;
            for (int i = startIndex; i < startIndex + materialCount; totalIndices += allIndices[i].Length, i++) { }

            mesh.SetIndexBufferParams(totalIndices, IndexFormat.UInt16);

            totalIndices = totalVertices = 0;
            for (int i = startIndex; i < startIndex + materialCount; totalVertices += allVertices[i].Length, totalIndices += allIndices[i].Length, i++)
            {
                mesh.SetIndexBufferData<ushort>(allIndices[i], 0, totalIndices, allIndices[i].Length, flags);
                mesh.SetSubMesh(i - startIndex,
                    new SubMeshDescriptor()
                    {
                        indexStart = totalIndices,
                        indexCount = allIndices[i].Length,
                        baseVertex = totalVertices,
                        topology = MeshTopology.Triangles
                    });
            }
            Vector3 modelSize = Vector3.one * component.meshSettings.chunkSize;
            mesh.bounds = new Bounds(modelSize * .5f + chunkPos * component.meshSettings.chunkSize, modelSize);

            updateMesh.End();
        }
    }
}
