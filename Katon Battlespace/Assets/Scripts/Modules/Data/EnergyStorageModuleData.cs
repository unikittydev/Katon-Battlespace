using Unity.Mathematics;

namespace Game.Modules.Data
{
    public class EnergyStorageModuleData : IModuleData
    {
        public int3 boundsStart { get; set; }
        public int3 boundsSize { get; set; }

        public float maxStorage = 30000f;
    }
}
