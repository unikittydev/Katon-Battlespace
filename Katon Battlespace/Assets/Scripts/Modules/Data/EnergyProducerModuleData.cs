using Unity.Mathematics;

namespace Game.Modules.Data
{
    public class EnergyProducerModuleData : IModuleData, IActionData
    {
        public int3 boundsStart { get; set; }
        public int3 boundsSize { get; set; }

        public float fuelCost = 200f;
        public float efficiency = 2000f;

        public string actionName { get; set; } = "Energy produce toggle";

    }
}
