using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Game.Voxels
{
    public class ColliderChunk : MonoBehaviour
    {
        private readonly List<BoxCollider> colliders = new List<BoxCollider>();

        private NativeList<float3> centers;
        private NativeList<float3> sizes;

        private static readonly ProfilerMarker updateCols = new ProfilerMarker("ColliderChunk.UpdateColliders");

        private void Awake()
        {
            centers = new NativeList<float3>(Allocator.Persistent);
            sizes = new NativeList<float3>(Allocator.Persistent);
        }

        private void OnDestroy()
        {
            centers.Dispose();
            sizes.Dispose();
        }

        public NativeList<float3> GetColliderCenters()
        {
            centers.Clear();
            return centers;
        }

        public NativeList<float3> GetColliderSizes()
        {
            sizes.Clear();
            return sizes;
        }

        public void UpdateColliders()
        {
            updateCols.Begin();

            int i;

            int newCollidersCount = centers.Length - colliders.Count;
            for (i = 0; i < newCollidersCount; i++)
                colliders.Add(gameObject.AddComponent<BoxCollider>());

            int length = centers.Length;

            for (i = 0; i < length; i++)
            {
                colliders[i].size = sizes[i];
                colliders[i].center = centers[i];
                colliders[i].enabled = true;
            }
            for (; i < colliders.Count; i++)
                colliders[i].enabled = false;

            updateCols.End();
        }
    }
}
