using Game.Modules.Data;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Game.Modules
{
    public class EnergyProducerModule : Module, IActionModule
    {
        private readonly ModuleContainer modules;

        private float fuelCost { get; set; }
        private float power { get; set; }

        private bool execute;

        public EnergyProducerModule(EnergyProducerModuleData data, ModuleContainer modules) : base(data.boundsStart, data.boundsSize)
        {
            this.modules = modules;

            fuelCost = data.fuelCost;
            power = data.efficiency;
        }

        public void OnPerformInput(CallbackContext _)
        {
            execute = !execute;
            if (execute)
                ModuleSystem.AddUpdateModule(Execute);
            else
                ModuleSystem.RemoveUpdateModule(Execute);
        }

        public void OnCancelInput(CallbackContext _) { }

        private void Execute()
        {
            int energyCount = modules.Count<EnergyStorageModule>(), fuelCount = modules.Count<FuelStorageModule>();

            float totalEnergySpace = 0f;
            for (int i = 0; i < energyCount; i++)
                totalEnergySpace += modules.Get<EnergyStorageModule>(i).canGet;

            float totalFuel = 0f;
            for (int i = 0; i < fuelCount; i++)
                totalFuel += modules.Get<FuelStorageModule>(i).canGive;

            float canProduceEnergy = Mathf.Min(totalEnergySpace, power);
            float canSpendFuel = Mathf.Min(totalFuel, fuelCost);

            float energyChangeFactor = canSpendFuel / fuelCost * Mathf.Clamp01(canProduceEnergy / totalEnergySpace) * Time.deltaTime;
            for (int i = 0; i < energyCount; i++)
            {
                var storage = modules.Get<EnergyStorageModule>(i);
                storage.ChangeEnergyStorage(storage.canGet * energyChangeFactor);
            }

            float fuelChangeFactor = canProduceEnergy / power * Mathf.Clamp01(canSpendFuel / totalFuel) * Time.deltaTime;
            for (int i = 0; i < fuelCount; i++)
            {
                var storage = modules.Get<FuelStorageModule>(i);
                storage.ChangeFuelStorage(-storage.canGive * fuelChangeFactor);
            }
        }

        public void Dispose()
        {
            if (execute)
                ModuleSystem.RemoveUpdateModule(Execute);
        }

        public override string ToString()
        {
            return $"{ModuleUIInfo.FuelConsumption}: {fuelCost:0}\n" +
                   $"{ModuleUIInfo.Power}: {power:0}";
        }
    }
}