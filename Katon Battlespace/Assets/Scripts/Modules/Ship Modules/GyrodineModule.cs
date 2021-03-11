using Game.Modules.Data;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Game.Modules
{
    public class GyrodineModule : Module, IActionModule
    {
        private readonly ModuleContainer modules;
        private Rigidbody parentRb;

        public float energyCost { get; }
        public Vector3 torque { get; }

        private float moveInput;

        public GyrodineModule(GyrodineModuleData data, ModuleContainer modules, Rigidbody parentRb) : base(data.boundsStart, data.boundsSize)
        {
            this.modules = modules;
            this.parentRb = parentRb;

            energyCost = data.energyCost;
            torque = data.torqueVector * data.defaultTorque;

            ModuleSystem.AddFixedUpdateModule(Execute);
        }

        public void OnPerformInput(CallbackContext ctx)
        {
            moveInput = Mathf.Clamp(ctx.ReadValue<float>(), -1f, 1f);
        }

        public void OnCancelInput(CallbackContext _)
        {
            moveInput = 0f;
        }

        private void Execute()
        {
            int energyCount = modules.Count<EnergyStorageModule>();

            float totalEnergy = 0f;
            for (int i = 0; i < energyCount; i++)
                totalEnergy += modules.Get<EnergyStorageModule>(i).canGive;

            float energySpending = Mathf.Abs(moveInput) * Mathf.Min(energyCost, totalEnergy) * Time.fixedDeltaTime;

            float spendFactor = Mathf.Clamp01(energySpending / totalEnergy);
            for (int i = 0; i < energyCount; i++)
            {
                var storage = modules.Get<EnergyStorageModule>(i);
                storage.ChangeEnergyStorage(-storage.canGive * spendFactor);
            }

            float appliedTorque = moveInput * energySpending / energyCost;
            parentRb.AddRelativeTorque(torque * appliedTorque, ForceMode.Force);
        }

        public void Dispose()
        {
            ModuleSystem.RemoveFixedUpdateModule(Execute);
        }

        public override string ToString()
        {
            return $"{ModuleUIInfo.EnergyConsumption}: {energyCost:0}\n" +
                   $"{ModuleUIInfo.Torque}: {torque:0}";
        }
    }
}
