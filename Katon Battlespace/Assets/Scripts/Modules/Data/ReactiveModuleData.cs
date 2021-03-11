using Unity.Mathematics;
using UnityEngine;

namespace Game.Modules.Data
{
    public class ReactiveModuleData : IModuleData, IActionData
    {
        public int3 boundsStart { get; set; }
        public int3 boundsSize { get; set; }

        public float fuelCost = 200f;
        public float defaultForce = 24000f;

        public Vector3 thrustVector;
        public Vector3 thrustPosition;

        public string actionName { get; set; }
    }
}