using Game.Modules.Data;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Game.Modules
{
    public class WeaponModule : Module, IActionModule
    {
        private readonly ModuleContainer modules;

        private Projectile prefab;

        private Rigidbody rb;

        public WeaponType type { get; }

        public float energyCost { get; }

        public float damage { get; }

        public float reloadTime { get; }
        private float _reloadCounter;

        public Vector3 forward { get; }
        public Vector3 shootPosition { get; }

        public WeaponModule(WeaponModuleData data, ModuleContainer modules, Rigidbody rb) : base(data.boundsStart, data.boundsSize)
        {
            this.modules = modules;
            this.rb = rb;

            damage = data.damage;
            energyCost = data.energyCost;
            reloadTime = data.reloadTime;

            forward = data.forward;
            shootPosition = data.shootPosition;

            type = data.type;

            prefab = Resources.Load<Projectile>("Prefabs/Projectile");

            ModuleSystem.AddUpdateModule(OnUpdate);
        }

        public void OnPerformInput(CallbackContext _)
        {
            Execute();
        }

        public void OnCancelInput(CallbackContext _)
        {

        }

        private void OnUpdate()
        {
            _reloadCounter += Time.deltaTime;
        }

        public void Execute()
        {
            if (_reloadCounter < reloadTime)
                return;
            _reloadCounter = 0f;

            int energyCount = modules.Count<EnergyStorageModule>();

            float totalEnergy = 0f;
            for (int i = 0; i < energyCount; i++)
                totalEnergy += modules.Get<EnergyStorageModule>(i).canGive;

            if (totalEnergy < energyCost)
                return;

            float spendFactor = Mathf.Clamp01(energyCost / totalEnergy);
            for (int i = 0; i < energyCount; i++)
            {
                var storage = modules.Get<EnergyStorageModule>(i);
                storage.ChangeEnergyStorage(-storage.canGive * spendFactor);
            }

            Projectile projectile = Object.Instantiate(prefab);
            projectile.transform.position = rb.transform.TransformPoint(shootPosition);
            projectile.transform.forward = rb.transform.TransformDirection(forward);
            projectile.rb.velocity = rb.velocity;
            projectile.rb.angularVelocity = rb.angularVelocity;
            projectile.rb.AddForce(projectile.transform.forward * projectile.startForce, ForceMode.Impulse);
        }

        public void Dispose()
        {
            ModuleSystem.RemoveUpdateModule(OnUpdate);
        }

        public override string ToString()
        {
            return $"{ModuleUIInfo.WeaponType}: {type}\n" +
                   $"{ModuleUIInfo.EnergyConsumption}: {energyCost:0}\n" +
                   $"{ModuleUIInfo.Damage}: {damage:0}\n" +
                   $"{ModuleUIInfo.ReloadTime}: {reloadTime:0}";
        }
    }
}
