using Game.Modules.Data;
using UnityEngine;

namespace Game.Modules
{
    public class EnergyStorageModule : Module
    {
        public float currStorage { get; private set; }
        public float maxStorage { get; private set; }

        public float canGive => currStorage;
        public float canGet => maxStorage - currStorage;

        public EnergyStorageModule(EnergyStorageModuleData data) : base(data.boundsStart, data.boundsSize)
        {
            maxStorage = currStorage = data.maxStorage;
        }

        public void ChangeEnergyStorage(float value)
        {
            currStorage = Mathf.Clamp(currStorage + value, 0, maxStorage);
        }

        public void TransferEnergy(ref EnergyStorageModule module, float value)
        {
            float canTransfer = Mathf.Min(value, canGive, module.canGet);
            currStorage -= canTransfer;
            module.currStorage += canTransfer;
        }

        public override string ToString()
        {
            return $"{ModuleUIInfo.Storage}: {maxStorage:0}";
        }
    }
}