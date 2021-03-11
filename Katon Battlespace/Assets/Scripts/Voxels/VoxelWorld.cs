using Game.Voxels.Editor;
using System;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Voxels
{
    public enum VoxelType
    {
        Content = 0,
        Health = 1,
    }

    public struct VoxelWorld : IDisposable
    {
        public const int voxelTypes = 2;

        private readonly NativeArray<byte>[] data;

        public int3 size { get; private set; }

        public int3 mul { get; private set; }

        public bool IsCreated => data != null;

        public int Length => data[0].Length;

        public VoxelWorld(in int3 size) : this(size, Allocator.Persistent) { }

        public VoxelWorld(in int3 size, Allocator allocator)
        {
            this.size = size;
            mul = new int3(size.y * size.z, size.z, 1);
            data = new NativeArray<byte>[voxelTypes];
            for (int i = 0; i < voxelTypes; i++)
                data[i] = new NativeArray<byte>(size.x * mul.x, allocator);
        }

        public VoxelWorld(in int3 size, byte[] content, byte[] health, Allocator allocator)
        {
            this.size = size;
            mul = new int3(size.y * size.z, size.z, 1);
            data = new NativeArray<byte>[voxelTypes];
            data[(int)VoxelType.Content] = new NativeArray<byte>(content, allocator);
            data[(int)VoxelType.Health] = new NativeArray<byte>(health, allocator);
        }

        public NativeArray<byte> this[VoxelType dataType]
        {
            get => data[(int)dataType];
        }

        public bool ContainsPosition(in int3 position)
        {
            return math.all((position >= 0) & (position < size));
        }

        public void SetContent(int index, byte value) => data[(int)VoxelType.Content][index] = value;

        public void SetContent(in int3 pos, byte value) => data[(int)VoxelType.Content][GetFlatIndex(in pos)] = value;

        public void SetHealth(int index, byte value) => data[(int)VoxelType.Health][index] = value;

        public void SetHealth(in int3 pos, byte value) => data[(int)VoxelType.Health][GetFlatIndex(in pos)] = value;

        public void SetVoxel(int index, byte content, byte health)
        {
            SetContent(index, content);
            SetHealth(index, health);
        }

        public void SetVoxel(in int3 pos, byte content, byte health)
        {
            int index = GetFlatIndex(in pos);
            SetVoxel(index, content, health);
        }

        public int GetFlatIndex(in int3 pos) => pos.x * mul.x + pos.y * mul.y + pos.z;

        public int GetFlatIndex(int x, int y, int z) => x * mul.x + y * mul.y + z;

        public void Resize(int3 start, int3 end)
        {
            int3 oldSize = size;

            size += end + start;
            mul = new int3(size.y * size.z, size.z, 1);

            VoxelWorld oldWorld = new VoxelWorld(oldSize);

            for (int i = 0; i < voxelTypes; i++)
            {
                oldWorld.data[i] = data[i];
                data[i] = new NativeArray<byte>(size.x * mul.x, Allocator.Persistent);
            }

            VoxelEditorTools.PasteArea(this, oldWorld, start, -start, start + oldSize);

            oldWorld.Dispose();
        }

        public void Dispose()
        {
            for (int i = 0; i < voxelTypes; i++)
                data[i].Dispose();
        }
    }
}
