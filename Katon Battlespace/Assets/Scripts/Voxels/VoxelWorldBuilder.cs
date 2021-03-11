using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Game.Voxels
{
    public class VoxelWorldBuilder : MonoBehaviour
    {
        private struct BuildSettings
        {
            /// <summary>
            /// Стартовая позиция чанка.
            /// </summary>
            public int3 startChunk;
            /// <summary>
            /// Конечная позиция чанка.
            /// </summary>
            public int3 endChunk;
            /// <summary>
            /// Размеры области в чанках.
            /// </summary>
            public int3 extends;
            /// <summary>
            /// Коэффициенты для преобразования позиции чанка в индекс.
            /// </summary>
            public int3 chunkMul;
            /// <summary>
            /// Длина одного чанка в вокселях.
            /// </summary>
            public int chunkSize;
            /// <summary>
            /// Стартовая позиция чанка в виде индекса.
            /// </summary>
            public int startIndex;

            public BuildSettings(in int3 start, in int3 end, in int3 worldSize, int chunkSize)
            {
                startChunk = start / chunkSize;
                endChunk = end / chunkSize;
                this.chunkSize = chunkSize;

                extends = (int3)math.ceil((float3)worldSize / chunkSize);
                chunkMul = new int3(extends.y * extends.z, extends.z, 1);

                startIndex = (startChunk.x * extends.y + startChunk.y) * extends.z + startChunk.z;
            }
        }
        
        private static VoxelWorldComponent component;

        private static NativeArray<int> backFaceTriangle;
        private static NativeArray<int> frontFaceTriangle;

        private static NativeArray<float3> faceNormals;
        private static NativeArray<half4> faceTangents;

        private static NativeArray<uint> colorPalette;

        private static NativeList<JobHandle> jobHandles;

        private static readonly List<NativeList<ushort>> allIndices = new List<NativeList<ushort>>();
        private static readonly List<NativeList<VoxelVertex>> allVertices = new List<NativeList<VoxelVertex>>();

        private static readonly ProfilerMarker buildWorldMarker = new ProfilerMarker("BuildVoxelWorld");
        private static readonly ProfilerMarker prepareMeshMarker = new ProfilerMarker("PrepareMeshBuilding");
        private static readonly ProfilerMarker prepareCollidersMarker = new ProfilerMarker("PrepareColliderBuilding");
        private static readonly ProfilerMarker meshJobCreationMarker = new ProfilerMarker("CreateMeshJobs");
        private static readonly ProfilerMarker collidersJobCreationMarker = new ProfilerMarker("CreateColliderJobs");
        private static readonly ProfilerMarker completeJobsMarker = new ProfilerMarker("CompleteJobs");
        private static readonly ProfilerMarker updateMeshesMarker = new ProfilerMarker("UpdateMeshes");
        private static readonly ProfilerMarker updateColsMarker = new ProfilerMarker("UpdateColliders");

        private static BuildSettings meshSettings;
        private static BuildSettings colliderSettings;

        private void Awake()
        {
            backFaceTriangle = new NativeArray<int>(new[] { -2, -3, -4, -1, -2, -4 }, Allocator.Persistent);
            frontFaceTriangle = new NativeArray<int>(new[] { -4, -3, -2, -4, -2, -1 }, Allocator.Persistent);

            faceNormals = new NativeArray<float3>(
                new[]
                {
                    new float3( 1f, 0f, 0f), new float3(0f,  1f, 0f), new float3(0f, 0f,  1f),
                    new float3(-1f, 0f, 0f), new float3(0f, -1f, 0f), new float3(0f, 0f, -1f)
                },
                Allocator.Persistent);

            faceTangents = new NativeArray<half4>(
                new[]
                {
                    (half4)new float4(0f, 0f, 1f, -1f), (half4)new float4(1f, 0f, 0f, -1f), (half4)new float4(0f, 1f, 0f, -1f),
                    (half4)new float4(0f, 0f, 1f,  1f), (half4)new float4(1f, 0f, 0f,  1f), (half4)new float4(0f, 1f, 0f,  1f)
                },
                Allocator.Persistent);

            colorPalette = new NativeArray<uint>(
                new[]
                {
                0b11111111_11000000_11000000_11000000, 0b11111111_00010000_00010000_11111111, 0b11111111_00000000_00000000_00000000, 0b11111111_11111111_11111111_11111111,
                0b11111111_11111111_00000000_00000000, 0b11111111_00000000_11111111_00000000, 0b11111111_11111111_00000000_11111111, 0b11111111_00000000_11111111_11111111,
                0b11111111_11111111_11111111_00000000, 0b11111111_01000000_11000000_00011110, 0b11111111_00111000_10000000_10000000, 0b11111111_11111111_10000000_10000000,
                0b11111111_10000000_11000000_10000000, 0b11111111_11100000_10000000_11110000, 0b11111111_10101010_10101010_10100101, 0b10100010_10100111_10111000_11010011,
                },
                Allocator.Persistent);

            jobHandles = new NativeList<JobHandle>(Allocator.Persistent);
        }

        private void OnDestroy()
        {
            backFaceTriangle.Dispose();
            frontFaceTriangle.Dispose();

            faceNormals.Dispose();
            faceTangents.Dispose();

            colorPalette.Dispose();
            jobHandles.Dispose();

            for (int i = 0; i < allIndices.Count; i++)
            {
                allIndices[i].Dispose();
                allVertices[i].Dispose();
            }
            allIndices.Clear();
            allVertices.Clear();
        }

        public static void BuildVoxelWorld(VoxelWorldComponent component, in int3 start, in int3 end)
        {
            buildWorldMarker.Begin();

            VoxelWorldBuilder.component = component;

            PrepareMeshBuilding(start, end);
            PrepareColliderBuilding(start, end);
            CreateMeshJobs();
            CreateColliderJobs();
            CompleteJobs();
            UpdateMeshes();
            UpdateColliders();

            buildWorldMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void PrepareMeshBuilding(in int3 start, in int3 end)
        {
            prepareMeshMarker.Begin();

            meshSettings = new BuildSettings(start, end, component.world.size, component.meshSettings.chunkSize);
            meshSettings.startChunk -= math.select(int3.zero, new int3(1, 1, 1), start > int3.zero & start % meshSettings.chunkSize == int3.zero);
            meshSettings.endChunk += math.select(int3.zero, new int3(1, 1, 1), (start < component.world.size - 1) & (start % meshSettings.chunkSize == meshSettings.chunkSize - 1));

            int3 updateBounds = meshSettings.endChunk - meshSettings.startChunk + 1;
            for (int i = allIndices.Count / MeshChunk.materialCount; i < updateBounds.x * updateBounds.y * updateBounds.z; i++)
                for (int s = 0; s < MeshChunk.materialCount; s++)
                {
                    allIndices.Add(new NativeList<ushort>(Allocator.Persistent));
                    allVertices.Add(new NativeList<VoxelVertex>(Allocator.Persistent));
                }

            prepareMeshMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void PrepareColliderBuilding(in int3 start, in int3 end)
        {
            prepareCollidersMarker.Begin();

            colliderSettings = new BuildSettings(start, end, component.world.size, component.colliderSettings.chunkSize);

            prepareCollidersMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CreateMeshJobs()
        {
            meshJobCreationMarker.Begin();

            for (int x = meshSettings.startChunk.x, xOff = meshSettings.startIndex; x <= meshSettings.endChunk.x; x++, xOff += meshSettings.chunkMul.x)
                for (int y = meshSettings.startChunk.y, yOff = xOff; y <= meshSettings.endChunk.y; y++, yOff += meshSettings.chunkMul.y)
                    for (int z = meshSettings.startChunk.z, zOff = yOff; z <= meshSettings.endChunk.z; z++, zOff += meshSettings.chunkMul.z)
                    {
                        int3 startPos = new int3(x, y, z) * meshSettings.chunkSize, endPos = math.min(startPos + meshSettings.chunkSize, component.world.size);
                        for (int s = 0; s < MeshChunk.materialCount; s++)
                        {
                            int listIndex = (zOff - meshSettings.startIndex) * MeshChunk.materialCount + s;
                            allIndices[listIndex].Clear();
                            allVertices[listIndex].Clear();

                            BuildSubmeshJob submeshJob = new BuildSubmeshJob()
                            {
                                voxels = component.world[VoxelType.Content],

                                colorPalette = colorPalette,
                                backFaceTriangle = backFaceTriangle,
                                frontFaceTriangle = frontFaceTriangle,

                                faceNormals = faceNormals,
                                faceTangents = faceTangents,

                                indices = allIndices[listIndex],
                                vertices = allVertices[listIndex],

                                mask = component.meshSettings.chunks[zOff].GetClearMask(s),
                                chunkSize = meshSettings.chunkSize,

                                id = s,
                                mul = component.world.mul,
                                startPos = startPos,
                                endPos = endPos,
                                worldSize = component.world.size
                            };
                            jobHandles.Add(submeshJob.Schedule());
                        }
                    }

            meshJobCreationMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CreateColliderJobs()
        {
            collidersJobCreationMarker.Begin();

            for (int x = colliderSettings.startChunk.x, xOff = colliderSettings.startIndex; x <= colliderSettings.endChunk.x; x++, xOff += colliderSettings.chunkMul.x)
                for (int y = colliderSettings.startChunk.y, yOff = xOff; y <= colliderSettings.endChunk.y; y++, yOff += colliderSettings.chunkMul.y)
                    for (int z = colliderSettings.startChunk.z, zOff = yOff; z <= colliderSettings.endChunk.z; z++, zOff += colliderSettings.chunkMul.z)
                    {
                        int3 startPos = new int3(x, y, z) * colliderSettings.chunkSize, endPos = math.min(startPos + colliderSettings.chunkSize, component.world.size);
                        BuildMeshCollidersJob collidersJob = new BuildMeshCollidersJob()
                        {
                            voxels = component.world[VoxelType.Content],
                            mask = new NativeArray<bool>(colliderSettings.chunkSize * colliderSettings.chunkSize * colliderSettings.chunkSize, Allocator.TempJob),

                            startPos = startPos,
                            mul = component.world.mul,
                            maskMul = new int3(colliderSettings.chunkSize * colliderSettings.chunkSize, colliderSettings.chunkSize, 1),
                            maxSize = endPos - startPos,

                            colliderCenters = component.colliderSettings.chunks[zOff].GetColliderCenters(),
                            colliderSizes = component.colliderSettings.chunks[zOff].GetColliderSizes()
                        };
                        jobHandles.Add(collidersJob.Schedule());
                    }

            collidersJobCreationMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CompleteJobs()
        {
            completeJobsMarker.Begin();
            JobHandle.CompleteAll(jobHandles);
            jobHandles.Clear();
            completeJobsMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UpdateMeshes()
        {
            updateMeshesMarker.Begin();

            for (int x = meshSettings.startChunk.x, xOff = meshSettings.startIndex; x <= meshSettings.endChunk.x; x++, xOff += meshSettings.chunkMul.x)
                for (int y = meshSettings.startChunk.y, yOff = xOff; y <= meshSettings.endChunk.y; y++, yOff += meshSettings.chunkMul.y)
                    for (int z = meshSettings.startChunk.z, zOff = yOff; z <= meshSettings.endChunk.z; z++, zOff += meshSettings.chunkMul.z)
                        component.meshSettings.chunks[zOff].UpdateMesh(component, allIndices, allVertices, new Vector3(x, y, z), (zOff - meshSettings.startIndex) * MeshChunk.materialCount);

            updateMeshesMarker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UpdateColliders()
        {
            updateColsMarker.Begin();

            Vector3 oldVel = component.rb.velocity;
            Vector3 oldAngularVel = component.rb.angularVelocity;
            bool wasKinematic = component.rb.isKinematic;
            component.rb.isKinematic = true;

            for (int x = colliderSettings.startChunk.x, xOff = colliderSettings.startIndex; x <= colliderSettings.endChunk.x; x++, xOff += colliderSettings.chunkMul.x)
                for (int y = colliderSettings.startChunk.y, yOff = xOff; y <= colliderSettings.endChunk.y; y++, yOff += colliderSettings.chunkMul.y)
                    for (int z = colliderSettings.startChunk.z, zOff = yOff; z <= colliderSettings.endChunk.z; z++, zOff += colliderSettings.chunkMul.z)
                        component.colliderSettings.chunks[zOff].UpdateColliders();

            component.rb.isKinematic = wasKinematic;
            component.rb.velocity = oldVel;
            component.rb.angularVelocity = oldAngularVel;

            updateColsMarker.End();
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct BuildSubmeshJob : IJob
        {
            [ReadOnly]
            public NativeArray<byte> voxels;
            [ReadOnly]
            public NativeArray<uint> colorPalette;

            [ReadOnly]
            public NativeArray<int> backFaceTriangle, frontFaceTriangle;

            [ReadOnly]
            public NativeArray<float3> faceNormals;
            [ReadOnly]
            public NativeArray<half4> faceTangents;

            [WriteOnly]
            public NativeList<ushort> indices;
            [WriteOnly]
            public NativeList<VoxelVertex> vertices;

            public NativeArray<byte> mask;

            [ReadOnly]
            public int id, chunkSize;
            private byte maskValue;
            private int verticesCount;
            [ReadOnly]
            public int3 startPos, endPos, worldSize, mul;

            public void Execute()
            {
                maskValue++;
                for (int f = 0; f < 6; f++)
                {
                    int dir = f % 3;
                    int ax1 = (f + 1) % 3;
                    int ax2 = (f + 2) % 3;
                    bool backFace = f > 2;
                    int dirOff = math.select(mul[dir], -mul[dir], backFace);

                    BuildSubmeshDirection(dir, ax1, ax2, dirOff, backFace, f);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void BuildSubmeshDirection(int dir, int ax1, int ax2, int dirOff, bool backFace, int f)
            {
                int3 start = startPos;
                if (backFace)
                {
                    BuildSubmeshSlice(dir, ax1, ax2, dirOff, backFace, start[dir] == 0, start, f);
                    for (start[dir]++; start[dir] < endPos[dir]; start[dir]++)
                        BuildSubmeshSlice(dir, ax1, ax2, dirOff, backFace, false, start, f);
                }
                else
                {
                    for (; start[dir] < endPos[dir] - 1; start[dir]++)
                        BuildSubmeshSlice(dir, ax1, ax2, dirOff, backFace, false, start, f);
                    BuildSubmeshSlice(dir, ax1, ax2, dirOff, backFace, start[dir] == (worldSize[dir] - 1), start, f);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void BuildSubmeshSlice(int dir, int ax1, int ax2, int dirOff, bool backFace, bool edge, int3 startPos, int f)
            {
                int3 currPos, quadSize, offsetPos;

                int sp0Off = startPos[dir] * mul[dir], sp1Off, sp2Off;
                int cp1Off, cp2Off;
                int smp1Off, smp2Off, cmp1Off, cmp2Off;

                maskValue++;

                int3 start = startPos;
                for (start[ax1] = startPos[ax1], sp1Off = sp0Off + start[ax1] * mul[ax1], smp1Off = 0; start[ax1] < endPos[ax1]; start[ax1]++, sp1Off += mul[ax1], smp1Off += chunkSize)
                    for (start[ax2] = startPos[ax2], sp2Off = sp1Off + start[ax2] * mul[ax2], smp2Off = smp1Off; start[ax2] < endPos[ax2]; start[ax2]++, sp2Off += mul[ax2], smp2Off++)
                    {
                        if (mask[smp2Off] == maskValue || voxels[sp2Off] == 0 || math.min(voxels[sp2Off] & 0x0F, MeshChunk.materialCount - 1) != id || !VoxelFaceVisible(edge, sp2Off, dirOff))
                            continue;
                        quadSize = new int3();

                        for (currPos = start, currPos[ax2]++, cp2Off = sp2Off + mul[ax2], cmp2Off = smp2Off + 1;
                            currPos[ax2] < endPos[ax2] && mask[cmp2Off] != maskValue && CanMergeVoxels(edge, sp2Off, cp2Off, dirOff);
                            currPos[ax2]++, cp2Off += mul[ax2], cmp2Off++) { }
                        quadSize[ax2] = currPos[ax2] - start[ax2];

                        for (currPos = start, currPos[ax1]++, cp1Off = sp1Off + mul[ax1], cmp1Off = smp1Off + chunkSize;
                            currPos[ax1] < endPos[ax1] && mask[cmp1Off] != maskValue && CanMergeVoxels(edge, sp1Off, cp1Off, dirOff);
                            currPos[ax1]++, cp1Off += mul[ax1], cmp1Off += chunkSize)
                        {
                            for (currPos[ax2] = start[ax2], cp2Off = cp1Off + currPos[ax2] * mul[ax2], cmp2Off = cmp1Off + (currPos[ax2] - start[ax2]) * chunkSize;
                                currPos[ax2] < endPos[ax2] && mask[cmp2Off] != maskValue && CanMergeVoxels(edge, sp2Off, cp2Off, dirOff);
                                currPos[ax2]++, cp2Off += mul[ax2], cmp2Off++) { }

                            if (currPos[ax2] - start[ax2] < quadSize[ax2])
                                break;
                            currPos[ax2] = start[ax2];
                        }
                        quadSize[ax1] = currPos[ax1] - start[ax1];

                        float3 normal = faceNormals[f];
                        half4 tangent = faceTangents[f];
                        uint color = colorPalette[voxels[sp2Off] >> 4];

                        offsetPos = start;
                        offsetPos[dir] += math.select(1, 0, backFace);

                        int material = voxels[sp2Off] & 0x0F;
                        half2 coords = (half2)(new float2(material & 0x03, material >> 2) * .25f);

                        vertices.Add(new VoxelVertex()
                        {
                            pos = offsetPos,
                            normal = normal,
                            tangent = tangent,
                            color = color,
                            uv = new half2(),
                            uv2 = coords
                        });

                        float3 tempVertice = offsetPos;
                        tempVertice[ax1] += quadSize[ax1];
                        vertices.Add(new VoxelVertex()
                        {
                            pos = tempVertice,
                            normal = normal,
                            tangent = tangent,
                            color = color,
                            uv = new half2((half)0, (half)quadSize[ax1]),
                            uv2 = coords
                        });

                        tempVertice = offsetPos + quadSize;
                        vertices.Add(new VoxelVertex()
                        {
                            pos = tempVertice,
                            normal = normal,
                            tangent = tangent,
                            color = color,
                            uv = new half2((half)quadSize[ax2], (half)quadSize[ax1]),
                            uv2 = coords
                        });

                        tempVertice = offsetPos;
                        tempVertice[ax2] += quadSize[ax2];
                        vertices.Add(new VoxelVertex()
                        {
                            pos = tempVertice,
                            normal = normal,
                            tangent = tangent,
                            color = color,
                            uv = new half2((half)quadSize[ax2], (half)0),
                            uv2 = coords
                        });

                        verticesCount += 4;

                        for (int i = 0; i < 6; i++)
                            indices.Add((ushort)(verticesCount + math.select(frontFaceTriangle[i], backFaceTriangle[i], backFace)));

                        for (int q1 = 0, q1Off = smp2Off; q1 < quadSize[ax1]; q1++, q1Off += chunkSize)
                            for (int q2 = 0, q2Off = q1Off; q2 < quadSize[ax2]; q2++, q2Off++)
                                mask[q2Off] = maskValue;
                    }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool VoxelFaceVisible(bool edge, int fromPos, int offset)
            {
                return edge || voxels[fromPos] != voxels[fromPos + offset] && voxels[fromPos] != 0 && (voxels[fromPos + offset] & 0x0F) == 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool CanMergeVoxels(bool edge, int fromPos, int toPos, int offset)
            {
                return voxels[fromPos] == voxels[toPos] && VoxelFaceVisible(edge, toPos, offset);
            }
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct BuildMeshCollidersJob : IJob
        {
            [ReadOnly]
            public NativeArray<byte> voxels;

            [DeallocateOnJobCompletion]
            public NativeArray<bool> mask;

            [ReadOnly]
            public int3 startPos, mul, maskMul, maxSize;
            public int3 currPos, colSize, chunkPos;

            [WriteOnly]
            public NativeList<float3> colliderCenters;
            [WriteOnly]
            public NativeList<float3> colliderSizes;

            public void Execute()
            {
                int sxwOff, sywOff, szwOff, sxmOff, symOff, szmOff;

                for (chunkPos.x = 0, sxwOff = startPos.x * mul.x,          sxmOff = 0;       chunkPos.x < maxSize.x; chunkPos.x++, sxwOff += mul.x, sxmOff += maskMul.x)
                    for (chunkPos.y = 0, sywOff = sxwOff + startPos.y * mul.y, symOff = sxmOff; chunkPos.y < maxSize.y; chunkPos.y++, sywOff += mul.y, symOff += maskMul.y)
                        for (chunkPos.z = 0, szwOff = sywOff + startPos.z * mul.z, szmOff = symOff; chunkPos.z < maxSize.z; chunkPos.z++, szwOff += mul.z, szmOff += maskMul.z)
                        {
                            if (!CanMerge(szmOff, szwOff))
                                continue;

                            currPos = chunkPos;
                            colSize.x = FindXSize(++currPos.x, szmOff + maskMul.x, szwOff + mul.x,                                                 maxSize.x);

                            currPos = chunkPos;
                            colSize.y = FindYSize(++currPos.y, szmOff + maskMul.y, szwOff + mul.y, chunkPos.x + colSize.x,                         maxSize.y);

                            currPos = chunkPos;
                            colSize.z = FindZSize(++currPos.z, szmOff + maskMul.z, szwOff + mul.z, chunkPos.x + colSize.x, chunkPos.y + colSize.y, maxSize.z);
                            
                            colliderSizes.Add(colSize);
                            colliderCenters.Add(startPos + chunkPos + (float3)colSize * .5f);

                            for (int q0 = 0, q0Off = szmOff; q0 < colSize.x; q0++, q0Off += maskMul.x)
                                for (int q1 = 0, q1Off = q0Off; q1 < colSize.y; q1++, q1Off += maskMul.y)
                                    for (int q2 = 0, q2Off = q1Off; q2 < colSize.z; q2++, q2Off += maskMul.z)
                                        mask[q2Off] = true;
                        }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool CanMerge(int maskIndex, int worldIndex)
            {
                return !mask[maskIndex] && voxels[worldIndex] != 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindXSize(int startX, int maskIndex, int worldIndex, int maxSize)
            {
                int xmOff, xwOff;
                for (
                    currPos.x = startX, xmOff = maskIndex, xwOff = worldIndex;
                    currPos.x < maxSize && CanMerge(xmOff, xwOff);
                    currPos.x++, xmOff += maskMul.x, xwOff += mul.x
                    ) { }
                return currPos.x - chunkPos.x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindYSize(int startY, int maskIndex, int worldIndex, int maxSizeX, int maxSizeY)
            {
                int ymOff, ywOff;
                for (
                    currPos.y = startY, ymOff = maskIndex, ywOff = worldIndex;
                    currPos.y < maxSizeY && FindXSize(chunkPos.x, ymOff, ywOff, maxSizeX) == colSize.x;
                    currPos.y++, ymOff += maskMul.y, ywOff += mul.y
                    ) { }
                return currPos.y - chunkPos.y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int FindZSize(int startZ, int maskIndex, int worldIndex, int maxSizeX, int maxSizeY, int maxSizeZ)
            {
                int zmOff, zwOff;
                for (
                    currPos.z = startZ, zmOff = maskIndex, zwOff = worldIndex;
                    currPos.z < maxSizeZ && FindYSize(chunkPos.y, zmOff, zwOff, maxSizeX, maxSizeY) == colSize.y;
                    currPos.z++, zmOff += maskMul.z, zwOff += mul.z
                    ) { }
                return currPos.z - chunkPos.z;
            }
        }
    }
}
