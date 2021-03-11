using Game.Voxels;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Modules.Creation
{
    public delegate bool MaterialPredicate(in VoxelWorld world, in NativeArray<bool> mask, in int index, ref Material material);

    public delegate bool ModuleGroupExecute(in VoxelWorld world, in NativeArray<bool> mask, in int3 position);

    public delegate bool RuleGroupExecute(in VoxelWorld world, in NativeArray<bool> mask);

    public static class ModuleTools
    {
        public static bool TryGetBlock(in VoxelWorld world, in NativeArray<bool> mask, in int index, ref Material material)
        {
            return !mask[index] && (Material)(world[VoxelType.Content][index] & 0x0F) == material;
        }

        public static bool IsSolidBlock(in VoxelWorld world, in NativeArray<bool> mask, in int index, ref Material material)
        {
            material = (Material)(world[VoxelType.Content][index] & 0x0F);
            return !mask[index] && material >= Material.Metall && material <= Material.Plastic;
        }

        public static void FillMaskValue(this NativeArray<bool> mask, in int3 start, in int3 size, in int3 mul, bool value)
        {
            int3 end = start + size;
            for (int x = start.x, xOff = x * mul.x; x < end.x; x++, xOff += mul.x)
                for (int y = start.y, yOff = y * mul.y + xOff; y < end.y; y++, yOff += mul.y)
                    for (int z = start.z, zOff = z * mul.z + yOff; z < end.z; z++, zOff += mul.z)
                        mask[zOff] = value;
        }

        public static int GetHealthSum(this NativeArray<byte> health, in int3 start, in int3 size, in int3 mul)
        {
            int sum = 0;
            int3 end = start + size;
            for (int x = start.x, xOff = x * mul.x; x < end.x; x++, xOff += mul.x)
                for (int y = start.y, yOff = y * mul.y + xOff; y < end.y; y++, yOff += mul.y)
                    for (int z = start.z, zOff = z * mul.z + yOff; z < end.z; z++, zOff += mul.z)
                        sum += health[zOff];
            return sum;
        }
    }
}
