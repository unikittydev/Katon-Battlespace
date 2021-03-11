using System;
using Unity.Mathematics;

namespace Game.Modules
{
    public abstract class Module
    {
        /// <summary>
        /// Список поддерживаемых типов модулей.
        /// </summary>
        public readonly static Type[] moduleTypes = new[]
        {
            typeof(EnergyProducerModule),
            typeof(EnergyStorageModule),
            typeof(FuelStorageModule),
            typeof(ReactiveModule),
            typeof(GyrodineModule),
            typeof(WeaponModule)
        };

        /// <summary> Координаты угла области модуля. </summary>
        public int3 boundsStart { get; }
        /// <summary> Размер области модуля. </summary>
        public int3 boundsSize { get; }

        public Module(int3 start, int3 size)
        {
            boundsStart = start;
            boundsSize = size;
        }

        public override abstract string ToString();
    }
}
