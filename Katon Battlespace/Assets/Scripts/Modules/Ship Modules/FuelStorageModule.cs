using Game.Modules.Data;
using UnityEngine;

namespace Game.Modules
{
    public class FuelStorageModule : Module
    {
        public float currStorage { get; private set; }
        public float maxStorage { get; private set; }

        public float canGive => currStorage;
        public float canGet => maxStorage - currStorage;

        public FuelStorageModule(FuelStorageModuleData data) : base(data.boundsStart, data.boundsSize)
        {
            currStorage = maxStorage = data.maxStorage;
        }

        public void ChangeFuelStorage(float value)
        {
            currStorage = Mathf.Clamp(currStorage + value, 0, maxStorage);
        }

        public void TransferFuel(FuelStorageModule module, float value)
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