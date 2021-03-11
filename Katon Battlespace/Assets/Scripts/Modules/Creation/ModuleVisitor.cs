using Game.Modules.Data;
using System;
using System.Collections.Generic;

namespace Game.Modules.Creation
{
    public static class ModuleVisitor
    {
        private readonly static Dictionary<Type, Func<Ship, IModuleData, Module>> constructors = new Dictionary<Type, Func<Ship, IModuleData, Module>>()
        {
            { typeof(EnergyProducerModuleData), (part, data) => new EnergyProducerModule(data as EnergyProducerModuleData, part.modules) },
            { typeof(EnergyStorageModuleData), (part, data) => new EnergyStorageModule(data as EnergyStorageModuleData) },
            { typeof(FuelStorageModuleData), (part, data) => new FuelStorageModule(data as FuelStorageModuleData) },
            { typeof(ReactiveModuleData), (part, data) => new ReactiveModule(data as ReactiveModuleData, part.modules, part.component.rb) },
            { typeof(GyrodineModuleData), (part, data) => new GyrodineModule(data as GyrodineModuleData, part.modules, part.component.rb) },
            { typeof(WeaponModuleData), (part, data) => new WeaponModule(data as WeaponModuleData, part.modules, part.component.rb) }
        };

        public static Module CreateModule(Ship ship, IModuleData data)
        {
            Module module = constructors[data.GetType()](ship, data);
            ship.modules.Add(module);
            return module;
        }
    }
}
