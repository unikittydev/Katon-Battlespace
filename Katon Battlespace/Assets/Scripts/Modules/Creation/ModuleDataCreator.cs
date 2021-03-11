using Game.Modules.Data;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Modules.Creation
{
    public class ModuleDataCreator : MonoBehaviour
    {
        private const float bMul = 2f / 255f;
        private const float aMul = -1f / 510f;

        /// <summary>
        /// Направления для движков и орудий.
        /// </summary>
        private static readonly Vector3[] directions = new Vector3[]
        {
            Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back
        };

        /// <summary>
        /// Названия ввода для гиродинов.
        /// </summary>
        private static readonly string[] gyrodineActionNames = new string[]
        {
            "X rotate", "Y rotate", "Z rotate"
        };

        /// <summary>
        /// Названия ввода для движков.
        /// </summary>
        private static readonly string[] reactiveActionNames = new string[]
        {
            "Xpos move", "Ypos move", "Zpos move", "Xneg move", "Yneg move", "Zneg move"
        };

        /// <summary>
        /// Какой материал нужен для орудия определённого типа.
        /// </summary>
        private static readonly Dictionary<Material, WeaponType> weaponTypeByMaterial = new Dictionary<Material, WeaponType>()
        {
            { Material.Pipes, WeaponType.Kinetic },
        };

        /// <summary>
        /// Коэффициенты параметров модулей.
        /// </summary>
        private static readonly Dictionary<Type, float2[]> upgradeParameters = new Dictionary<Type, float2[]>()
        {
            { typeof(EnergyProducerRule), new [] { new float2(5f, 0f), new float2(2f, 1f) } },
            { typeof(EnergyStorageRule),  new [] { new float2(2f, 1f) } },
            { typeof(FuelStorageRule),    new [] { new float2(2f, 1f) } },
            { typeof(GyrodineRule),       new [] { new float2(3f, 0.5f), new float2(5f, 0f), new float2(5f, 0f), new float2(5f, 0f) } },
            { typeof(ReactiveRule),       new [] { new float2(5f, 0f), new float2(3f, 1f), new float2(3f, 0.5f) } },
            { typeof(WeaponRule),         new [] { new float2(3f, 0.2f), new float2(3f, 0.3f), new float2(3f, 1f) } },
        };

        /// <summary>
        /// Методы для создания данных модулей.
        /// </summary>
        private static readonly Dictionary<Type, Action<Ship, ModuleGroup, float[]>> constructors = new Dictionary<Type, Action<Ship, ModuleGroup, float[]>>()
        {
            { typeof(EnergyProducerRule), CreateEnergyProducer },
            { typeof(EnergyStorageRule), CreateEnergyStorage },
            { typeof(FuelStorageRule), CreateFuelStorage },
            { typeof(GyrodineRule), CreateGyrodine },
            { typeof(ReactiveRule), CreateReactive },
            { typeof(WeaponRule), CreateWeapon }
        };

        public static void CreateModulesFromGroup(Ship ship, ModuleGroup group)
        {
            float[] factors = new float[group.health.Length];
            for (int i = 0; i < factors.Length; i++)
                factors[i] = GetFactor(upgradeParameters[group.GetType()][i], group.health[i]);

            ship.modules.mask.FillMaskValue(group.boundsStart, group.boundsSize, ship.component.world.mul, true);

            constructors[group.GetType()](ship, group, factors);
        }

        private static float GetFactor(float2 parameters, int health) => GetFactor(parameters.x, parameters.y, health);

        private static float GetFactor(float maxHeight, float sqMultiplier, int health)
        {
            float b = (1f - 0.5f * sqMultiplier) * bMul * (maxHeight - 1f);
            float a = (1 - sqMultiplier) * aMul * b;
            return a * health * health + b * health + 1f;
        }

        private static void CreateEnergyProducer(Ship ship, ModuleGroup group, float[] factors)
        {
            var data = new EnergyProducerModuleData();
            data.fuelCost *= factors[0];
            data.efficiency *= factors[1];

            data.boundsStart = group.boundsStart;
            data.boundsSize = group.boundsSize;

            ModuleVisitor.CreateModule(ship, data);
        }

        private static void CreateEnergyStorage(Ship ship, ModuleGroup group, float[] factors)
        {
            var data = new EnergyStorageModuleData();
            data.maxStorage *= factors[0];

            data.boundsStart = group.boundsStart;
            data.boundsSize = group.boundsSize;

            ModuleVisitor.CreateModule(ship, data);
        }

        private static void CreateFuelStorage(Ship ship, ModuleGroup group, float[] factors)
        {
            var data = new FuelStorageModuleData();
            data.maxStorage *= factors[0];

            data.boundsStart = group.boundsStart;
            data.boundsSize = group.boundsSize;

            ModuleVisitor.CreateModule(ship, data);
        }

        private static void CreateGyrodine(Ship ship, ModuleGroup group, float[] factors)
        {
            for (int i = 0; i < group.ruleList.Length - 1; i++)
            {
                if (group.ruleList[i + 1].totalHealth == 0)
                    continue;

                var data = new GyrodineModuleData();
                float optFactor = factors[2];
                data.energyCost *= factors[0] * optFactor;
                data.defaultTorque *= factors[1] * optFactor;

                data.torqueVector = directions[i];
                data.actionName = gyrodineActionNames[i];

                data.boundsStart = group.boundsStart;
                data.boundsSize = group.boundsSize;

                if (ship.controller != null)
                    ship.controller.InitInput(data, ModuleVisitor.CreateModule(ship, data) as IActionModule);
            }
        }

        private static void CreateReactive(Ship ship, ModuleGroup group, float[] factors)
        {
            ReactiveRule rule = group as ReactiveRule;

            var data = new ReactiveModuleData();
            float optFactor = factors[1];
            data.fuelCost *= factors[0] * optFactor;
            data.defaultForce *= factors[2] * optFactor;

            data.thrustVector = -(float3)rule.dir;
            data.thrustPosition = (float3)(rule.boundsStart + math.select(int3.zero, rule.boundsSize - new int3(1, 1, 1), rule.i > 2)) + .5f;
            data.actionName = reactiveActionNames[rule.i];

            data.boundsStart = group.boundsStart;
            data.boundsSize = group.boundsSize;

            if (ship.controller != null)
                ship.controller.InitInput(data, ModuleVisitor.CreateModule(ship, data) as IActionModule);
        }

        private static void CreateWeapon(Ship ship, ModuleGroup group, float[] factors)
        {
            WeaponRule rule = group as WeaponRule;

            var data = new WeaponModuleData();
            float optFactor = factors[2];
            data.reloadTime *= factors[0];
            data.damage *= factors[1] * optFactor;
            data.energyCost *= optFactor;

            data.type = weaponTypeByMaterial[rule.material];
            data.forward = (float3)rule.dir;
            data.shootPosition = (float3)(rule.boundsStart + math.select(new int3(1, 1, 1), rule.boundsSize, rule.i > 2)) + rule.dir + .5f;

            data.boundsStart = group.boundsStart;
            data.boundsSize = group.boundsSize;

            if (ship.controller != null)
                ship.controller.InitInput(data, ModuleVisitor.CreateModule(ship, data) as IActionModule);
        }
    }
}
