using Game.Voxels.Data;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Voxels
{
    [RequireComponent(typeof(Rigidbody))]
    public class VoxelWorldComponent : MonoBehaviour, IGameData
    {
        [SerializeField]
        private ChunkSettings<MeshChunk> _meshSettings;
        public ChunkSettings<MeshChunk> meshSettings => _meshSettings;
        [SerializeField]
        private ChunkSettings<ColliderChunk> _colliderSettings;
        public ChunkSettings<ColliderChunk> colliderSettings => _colliderSettings;

        [SerializeField]
        private Rigidbody _rb = null;
        public Rigidbody rb => _rb;

        public VoxelWorld world;

        public void OnLoad(string json)
        {
            VoxelWorldData data = JsonUtility.FromJson<VoxelWorldData>(json);
            if (world.IsCreated)
                world.Dispose();
            world = new VoxelWorld(data.size, data.content, data.health, Unity.Collections.Allocator.Persistent);

            name = data.stats.name;
            rb.mass = data.stats.voxelCount;
            transform.position = data.position;
            UpdateChunks();
        }

        public string OnSave()
        {
            return JsonUtility.ToJson(new VoxelWorldData
            {
                size = world.size,
                content = world[VoxelType.Content].ToArray(),
                health = world[VoxelType.Health].ToArray(),
                stats = new Editor.Data.ShipStatsData()
                {
                    name = name,
                    voxelCount = (int)math.round(rb.mass),
                },
                position = transform.position
            });
        }

        private void OnDestroy()
        {
            world.Dispose();
        }

        public void ResizeWorld(in int3 start, in int3 end)
        {
            world.Resize(start, end);
            transform.Translate(-(float3)start);
            UpdateChunks();
        }

        public void UpdateChunks()
        {
            UpdateMeshChunks();
            UpdateColliderChunks();
            VoxelWorldBuilder.BuildVoxelWorld(this, int3.zero, world.size - 1);
        }

        private void UpdateMeshChunks()
        {
            int oldMeshChunkCount = meshSettings.chunks.Count;
            meshSettings.Update(world.size);
            int newMeshChunkCount = meshSettings.chunks.Count;

            for (int i = newMeshChunkCount - 1; i >= oldMeshChunkCount; i--)
                meshSettings.chunks[i].CreateMasks(meshSettings.chunkSize * meshSettings.chunkSize);
        }

        private void UpdateColliderChunks()
        {
            colliderSettings.Update(world.size);
        }
    }
}
