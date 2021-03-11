using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Voxels
{
    [System.Serializable]
    public class ChunkSettings<T> where T : Component
    {
        [SerializeField]
        private int _chunkSize;
        public int chunkSize => _chunkSize;

        [SerializeField]
        private T _prefab;
        public T prefab => _prefab;

        [SerializeField]
        private Transform _parent;
        public Transform parent => _parent;

        public List<T> chunks { get; } = new List<T>();

        public void Update(in int3 worldSize)
        {
            int3 extends = (int3)math.ceil((float3)worldSize / chunkSize);
            int chunkCount = extends.x * extends.y * extends.z;
            int startChunkCount = chunks.Count;

            for (int i = 0; i < chunkCount - startChunkCount; i++)
            {
                T chunk = Object.Instantiate(prefab, parent);
                chunks.Add(chunk);
            }
            for (int i = startChunkCount - 1; i > chunkCount - 1; i--)
            {
                Object.Destroy(chunks[i].gameObject);
                chunks.RemoveAt(i);
            }
        }
    }
}
