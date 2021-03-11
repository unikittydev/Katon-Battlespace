using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Voxels
{
    public class VoxelLineBuilder : MonoBehaviour
    {
        private static NativeList<int3> line;

        private static NativeList<int3> baseLine1;
        private static NativeList<int3> baseLine2;

        [BurstCompile]
        private struct BuildLineJob : IJob
        {
            public const float offset = 0.001f;

            [WriteOnly]
            public NativeList<int3> line;

            [ReadOnly]
            public float3 p1, p2;

            public void Execute()
            {
                float3 p2 = this.p2 + offset;

                int3 p1Int = (int3)math.floor(p1);
                int3 p2Int = (int3)math.floor(p2);

                int sx = p2Int.x > p1Int.x ? 1 : p2Int.x < p1Int.x ? -1 : 0;
                int sy = p2Int.y > p1Int.y ? 1 : p2Int.y < p1Int.y ? -1 : 0;
                int sz = p2Int.z > p1Int.z ? 1 : p2Int.z < p1Int.z ? -1 : 0;

                int3 g = p1Int;

                var gxp = p1Int.x + (p2Int.x > p1Int.x ? 1 : 0);
                var gyp = p1Int.y + (p2Int.y > p1Int.y ? 1 : 0);
                var gzp = p1Int.z + (p2Int.z > p1Int.z ? 1 : 0);

                var vx = p2.x == p1.x ? 1 : p2.x - p1.x;
                var vy = p2.y == p1.y ? 1 : p2.y - p1.y;
                var vz = p2.z == p1.z ? 1 : p2.z - p1.z;

                var vxvy = vx * vy;
                var vxvz = vx * vz;
                var vyvz = vy * vz;

                var errx = (gxp - p1.x) * vyvz;
                var erry = (gyp - p1.y) * vxvz;
                var errz = (gzp - p1.z) * vxvy;

                var derrx = sx * vyvz;
                var derry = sy * vxvz;
                var derrz = sz * vxvy;

                var testEscape = 100;
                do
                {
                    line.Add(g);

                    if (g.x == p2Int.x && g.y == p2Int.y && g.z == p2Int.z) break;

                    var xr = math.abs(errx);
                    var yr = math.abs(erry);
                    var zr = math.abs(errz);

                    if (sx != 0 && (sy == 0 || xr < yr) && (sz == 0 || xr < zr))
                    {
                        g.x += sx;
                        errx += derrx;
                    }
                    else if (sy != 0 && (sz == 0 || yr < zr))
                    {
                        g.y += sy;
                        erry += derry;
                    }
                    else if (sz != 0)
                    {
                        g.z += sz;
                        errz += derrz;
                    }
                } while (--testEscape > 0);

                if (testEscape == 0)
                    Debug.LogError("Error at line building in LineBuilder class");
            }
        }

        private void Awake()
        {
            line = new NativeList<int3>(Allocator.Persistent);

            baseLine1 = new NativeList<int3>(Allocator.Persistent);
            baseLine2 = new NativeList<int3>(Allocator.Persistent);
        }

        private void OnDestroy()
        {
            line.Dispose();

            baseLine1.Dispose();
            baseLine2.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeList<int3> BuildVoxelPoint(float3[] points)
        {
            line.Clear();
            line.Add(new int3(points[0]));
            return line;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeList<int3> BuildVoxelLine(float3[] points)
        {
            return BuildVoxelLine(points[0], points[1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeList<int3> BuildVoxelLine(float3 p1, float3 p2)
        {
            line.Clear();
            BuildLineJob lineJob = new BuildLineJob()
            {
                line = line,
                p1 = p1,
                p2 = p2
            };
            lineJob.Schedule().Complete();

            return line;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeList<int3> BuildVoxelTriangle(float3[] points)
        {
            return BuildVoxelTriangle(points[0], points[1], points[2]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeList<int3> BuildVoxelTriangle(float3 p1, float3 p2, float3 p3)
        {
            line.Clear();
            baseLine1.Clear();

            BuildLineJob baseLineJob = new BuildLineJob()
            {
                line = baseLine1,
                p1 = p1,
                p2 = p2
            };

            baseLineJob.Schedule().Complete();

            for (int i = 0; i < baseLine1.Length; i++)
            {
                BuildLineJob job = new BuildLineJob()
                {
                    line = line,
                    p1 = baseLine1[i],
                    p2 = p3
                };
                job.Schedule().Complete();
            }

            return line;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeList<int3> BuildVoxelQuad(float3[] points)
        {
            return BuildVoxelQuad(points[0], points[1], points[2], points[3]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeList<int3> BuildVoxelQuad(float3 p1, float3 p2, float3 p3, float3 p4)
        {
            line.Clear();
            baseLine1.Clear();
            baseLine2.Clear();

            JobHandle handle;

            BuildLineJob baseLine1Job = new BuildLineJob()
            {
                line = baseLine1,
                p1 = p1,
                p2 = p2
            };
            handle = baseLine1Job.Schedule();

            BuildLineJob baseLine2Job = new BuildLineJob()
            {
                line = baseLine2,
                p1 = p3,
                p2 = p4
            };
            handle = baseLine2Job.Schedule(handle);
            handle.Complete();

            for (int i = 0; i < math.min(baseLine1.Length, baseLine2.Length); i++)
            {
                BuildLineJob job = new BuildLineJob()
                {
                    line = line,
                    p1 = baseLine1[i],
                    p2 = baseLine2[i]
                };
                job.Schedule().Complete();
            }

            return line;
        }
    }
}
