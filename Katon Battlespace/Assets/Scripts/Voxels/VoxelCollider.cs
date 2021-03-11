using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Voxels
{
    public class VoxelCollider : MonoBehaviour
    {
        private ApplyDamageJob damageJob;

        private readonly static Func<float3[], NativeList<int3>>[] processMethods = new Func<float3[], NativeList<int3>>[]
                { VoxelLineBuilder.BuildVoxelPoint, VoxelLineBuilder.BuildVoxelLine, VoxelLineBuilder.BuildVoxelTriangle, VoxelLineBuilder.BuildVoxelQuad };

        private readonly ContactPoint[] contacts = new ContactPoint[4];
        private readonly float3[] points = new float3[4];

        private const float minSqrMagnitude = 10f;
        private const float minDamageEnergy = 2000f;

        private const float damagePerVoxelFactor = .003f;
        private const float fullDamageFactor = 5000f;

        private Transform tr;
        private Rigidbody rb;

        private VoxelWorldComponent world;

        private int collisionCount;

        private float initialMass;
        private Vector3 initialVelocity;
        private Vector3 initialAngularVelocity;

        private float kineticEnergySum;
        private float fullDamageEnergy;

        private NativeList<int> indicesList;

        private List<Collider> explosions = new List<Collider>();

        private void Start()
        {
            world = GetComponent<VoxelWorldComponent>();
            tr = transform;
            rb = GetComponent<Rigidbody>();

            indicesList = new NativeList<int>(Allocator.Persistent);

            damageJob = new ApplyDamageJob()
            {
                indices = indicesList,
                content = world.world[VoxelType.Content],
                health = world.world[VoxelType.Health]
            };
        }

        private void OnDestroy()
        {
            if (indicesList.IsCreated)
                indicesList.Dispose();
        }

        private void FixedUpdate()
        {
            if (kineticEnergySum > minDamageEnergy)
            {
                float E = kineticEnergySum / collisionCount;
                int damagePerVoxel = (int)(E * damagePerVoxelFactor / indicesList.Length);

                damageJob.damage = new NativeArray<int>(indicesList.Length, Allocator.TempJob);
                damageJob.damagePerVoxel = damagePerVoxel;
                damageJob.ScheduleParallel(indicesList.Length, 16, default).Complete();

                VoxelDestruction.DestructWorld(world);

                for (int i = 0; i < indicesList.Length; i++)
                    fullDamageEnergy += damageJob.damage[i];

                float k = Mathf.Sqrt(Mathf.Max(0f, E - fullDamageFactor * fullDamageEnergy) / Mathf.Max(1f, E));

                rb.AddForce(k * initialVelocity - rb.velocity, ForceMode.VelocityChange);
                rb.AddTorque(k * initialAngularVelocity - rb.angularVelocity, ForceMode.VelocityChange);

                collisionCount = 0;
                fullDamageEnergy = 0f;
                kineticEnergySum = 0f;

                indicesList.Clear();
                damageJob.damage.Dispose();
            }
            else
            {
                initialMass = rb.mass;
                initialVelocity = rb.velocity;
                initialAngularVelocity = rb.angularVelocity;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (GameSettings.processCollisions)
                ProcessCollision(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!explosions.Contains(other))
            {
                explosions.Add(other);
                explosions.RemoveAll(item => item = null);
                VoxelDamage.DealExplosionDamage(world, other.transform.position, other.GetComponent<Explosion>().damage);
            }
        }

        private void ProcessCollision(Collision collision)
        {
            if (collision.relativeVelocity.sqrMagnitude > minSqrMagnitude)
            {
                collision.GetContacts(contacts);

                for (int i = 0; i < collision.contactCount; i++)
                    points[i] = tr.InverseTransformPoint(contacts[i].point + contacts[i].normal * contacts[i].separation);

                NativeList<int3> voxelPositions = processMethods[collision.contactCount - 1](points);

                ApplyDamage(voxelPositions, collision);
            }
        }

        private void ApplyDamage(NativeList<int3> voxels, Collision collision)
        {
            kineticEnergySum += initialMass * initialVelocity.sqrMagnitude / 2f;
            if (collision.gameObject.TryGetComponent(out VoxelCollider other))
                kineticEnergySum += other.initialMass * other.initialVelocity.sqrMagnitude / 2f;

            collisionCount++;

            for (int i = 0; i < voxels.Length; i++)
            {
                int3 pos = voxels[i];
                if (pos.x < 0 || pos.x > world.world.size.x - 1 ||
                    pos.y < 0 || pos.y > world.world.size.y - 1 ||
                    pos.z < 0 || pos.z > world.world.size.z - 1)
                    continue;
                int index = world.world.GetFlatIndex(in pos);
                if (!indicesList.Contains(index))
                    indicesList.Add(index);
            }
        }

        [BurstCompile]
        private struct ApplyDamageJob : IJobFor
        {
            [ReadOnly]
            public NativeList<int> indices;

            [NativeDisableParallelForRestriction]
            public NativeArray<byte> content;
            [NativeDisableParallelForRestriction]
            public NativeArray<byte> health;

            [WriteOnly, NativeDisableParallelForRestriction]
            public NativeArray<int> damage;

            [ReadOnly]
            public int damagePerVoxel;

            public void Execute(int index)
            {
                byte diff = (byte)math.min(health[indices[index]], damagePerVoxel);

                health[indices[index]] = (byte)(health[indices[index]] - diff);
                content[indices[index]] = (byte)math.select(0, content[indices[index]], health[indices[index]] != 0);

                damage[index] = diff;
            }
        }
    }
}