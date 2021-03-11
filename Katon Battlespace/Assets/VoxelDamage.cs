using Game;
using Game.Voxels;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class VoxelDamage : MonoBehaviour
{
    private static VoxelWorldComponent component;

    public const float damageFactor = 10f;
    private const float damageRandomPercent = 0.1f;

    private static int3 start, end;

    public static void DealExplosionDamage(Ship ship, in Vector3 position, float power)
    {
        DealExplosionDamage(ship.component, position, power);
        ship.modules.Update();
    }

    public static void DealExplosionDamage(VoxelWorldComponent component, in Vector3 position, float power)
    {
        VoxelDamage.component = component;
        DamageJob((int3)(float3)component.transform.InverseTransformPoint(position), power);

        VoxelWorldBuilder.BuildVoxelWorld(component, start, end - 1);
        VoxelDestruction.DestructWorld(component);
    }

    private static void DamageJob(in int3 position, float power)
    {
        int radius = (int)(power / damageFactor);

        start = math.max(position - radius, int3.zero);
        end = math.min(start + radius + radius, component.world.size);

        DealDamageJob job = new DealDamageJob()
        {
            content = component.world[VoxelType.Content],
            health = component.world[VoxelType.Health],

            size = component.world.size,
            mul = component.world.mul,

            start = start,
            center = position,
            end = end,
            radius = radius,
            massDelta = new NativeArray<int>(1, Allocator.TempJob),

            random = new Random()
        };
        job.Schedule().Complete();
        component.rb.mass -= job.massDelta[0];
        job.massDelta.Dispose();
    }

    [BurstCompile]
    private struct DealDamageJob : IJob
    {
        public NativeArray<byte> content;
        public NativeArray<byte> health;

        [ReadOnly]
        public int3 size, mul;

        [ReadOnly]
        public int3 start, center, end;
        [ReadOnly]
        public int radius;

        [ReadOnly]
        public Random random;

        public NativeArray<int> massDelta;

        public void Execute()
        {
            random.InitState();
            int radiussq = radius * radius;
            int massDeltaCounter = 0;

            float minDamageFactor = 1f - damageRandomPercent, maxDamageFactor = 1f + damageRandomPercent;

            for (int x = start.x, xOff = x * mul.x; x < end.x; x++, xOff += mul.x)
                for (int y = start.y, yOff = xOff + y * mul.y; y < end.y; y++, yOff += mul.y)
                    for (int z = start.z, zOff = yOff + z * mul.z; z < end.z; z++, zOff += mul.z)
                    {
                        int3 offset = new int3(x, y, z) - center;
                        if (math.dot(offset, offset) <= radiussq)
                        {
                            int3 distance = radius - offset;
                            int damage = (int)(damageFactor * math.dot(distance, distance) * random.NextFloat(minDamageFactor, maxDamageFactor));
                            health[zOff] = (byte)math.max(health[zOff] - damage, 0);
                            massDeltaCounter += math.select(0, 1, health[zOff] == 0 && content[zOff] != 0);
                            content[zOff] = (byte)math.select(content[zOff], 0, health[zOff] == 0);
                        }
                    }
            massDelta[0] = massDeltaCounter;
        }
    }
}
