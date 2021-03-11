using Unity.Mathematics;
using UnityEngine;

namespace Game.Modules.Data
{
    public class GyrodineModuleData : IModuleData, IActionData
    {
        public int3 boundsStart { get; set; }
        public int3 boundsSize { get; set; }

        public float energyCost = 50f;
        public float defaultTorque = 18000f;

        public Vector3 torqueVector;

        public string actionName { get; set; }
    }
}
