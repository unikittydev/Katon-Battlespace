using Game.Voxels;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Modules.Creation
{
    public class ModuleFinder : MonoBehaviour
    {
        private static Dictionary<Material, ModuleGroup> moduleGroups = new Dictionary<Material, ModuleGroup>()
        {
            { Material.EnergyBlock, new EnergyStorageRule()  },
            { Material.FuelTank,    new FuelStorageRule()    },
            { Material.Glowstone,   new EnergyProducerRule() },
            { Material.Pipes,       new ReactiveRule()       },
            { Material.Computer,    new WeaponRule()         },
            { Material.Coil,        new GyrodineRule()       },
        };

        public static void FindModules(Ship ship)
        {
            FindModules(ship, int3.zero, ship.component.world.size);
        }

        public static void FindModules(Ship ship, in int3 start, in int3 size)
        {
            VoxelWorld world = ship.component.world;
            NativeArray<bool> mask = ship.modules.mask;

            int3 pos = new int3();
            int3 end = start + size;
            int xOff, yOff, zOff;

            for (pos.x = start.x, xOff =        pos.x * world.mul.x; pos.x < end.x; pos.x++, xOff += world.mul.x)
                for (pos.y = start.y, yOff = xOff + pos.y * world.mul.y; pos.y < end.y; pos.y++, yOff += world.mul.y)
                    for (pos.z = start.z, zOff = yOff + pos.z * world.mul.z; pos.z < end.z; pos.z++, zOff += world.mul.z)
                    {
                        Material mat = (Material)(world[VoxelType.Content][zOff] & 0x0F);
                        if (moduleGroups.ContainsKey(mat) && moduleGroups[mat].Execute(world, mask, pos))
                            ModuleDataCreator.CreateModulesFromGroup(ship, moduleGroups[mat]);
                    }
        }
    }
}
