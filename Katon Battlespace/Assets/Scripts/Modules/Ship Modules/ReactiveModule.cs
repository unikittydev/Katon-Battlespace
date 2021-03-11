using Game.Modules.Data;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Game.Modules
{
    public class ReactiveModule : Module, IActionModule
    {
        private readonly ModuleContainer modules;

        private Rigidbody parentRb;

        private ParticleSystem.MainModule particles;

        private float fuelCost { get; set; }
        private Vector3 forceVector { get; set; }

        private float moveInput;

        public ReactiveModule(ReactiveModuleData data, ModuleContainer modules, Rigidbody parentRb) : base(data.boundsStart, data.boundsSize)
        {
            this.modules = modules;
            this.parentRb = parentRb;

            fuelCost = data.fuelCost;
            forceVector = data.defaultForce * data.thrustVector;

            particles = Object.Instantiate(Resources.Load<ParticleSystem>("Prefabs/Reactive Trail"), parentRb.transform.TransformPoint(data.thrustPosition), Quaternion.LookRotation(-data.thrustVector), parentRb.transform.GetChild(3)).main;
            particles.startLifetimeMultiplier = 0f;
            
            ModuleSystem.AddFixedUpdateModule(Execute);
        }

        public void OnPerformInput(CallbackContext ctx)
        {
            particles.startLifetimeMultiplier = moveInput = Mathf.Clamp01(ctx.ReadValue<float>());
        }

        public void OnCancelInput(CallbackContext _)
        {
            particles.startLifetimeMultiplier = moveInput = 0f;
        }

        private void Execute()
        {
            int fuelCount = modules.Count<FuelStorageModule>();

            float totalFuel = 0f;
            for (int i = 0; i < fuelCount; i++)
                totalFuel += modules.Get<FuelStorageModule>(i).canGive;

            float canSpend = Mathf.Abs(moveInput) * Mathf.Min(fuelCost, totalFuel) * Time.fixedDeltaTime;

            float spendFactor = Mathf.Clamp01(canSpend / totalFuel);
            for (int i = 0; i < fuelCount; i++)
            {
                var storage = modules.Get<FuelStorageModule>(i);
                storage.ChangeFuelStorage(-storage.canGive * spendFactor);
            }

            float forceFactor = moveInput * canSpend / fuelCost;
            parentRb.AddRelativeForce(forceVector * forceFactor, ForceMode.Force);

            particles.startLifetimeMultiplier = Mathf.Lerp(particles.startLifetimeMultiplier, forceFactor / Time.fixedDeltaTime, Time.deltaTime);
        }

        public void Dispose()
        {
            ModuleSystem.RemoveFixedUpdateModule(Execute);
        }

        public override string ToString()
        {
            return $"{ModuleUIInfo.FuelConsumption}: {fuelCost:0}\n" +
                   $"{ModuleUIInfo.Force}: {forceVector:0}";
        }
    }
}
