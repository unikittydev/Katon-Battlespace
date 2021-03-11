using Unity.Mathematics;
using UnityEngine;

namespace Game.Modules.Data
{
    public enum WeaponType
    {
        Kinetic,
        Energy,
        Rocket
    }

    public class WeaponModuleData : IModuleData, IActionData
    {
        public int3 boundsStart { get; set; }
        public int3 boundsSize { get; set; }

        public WeaponType type;

        public float reloadTime = 2f;
        public float damage = 10f;
        public float energyCost = 100f;

        public Vector3 forward;
        public Vector3 shootPosition;

        public string actionName { get; set; } = "Shoot";
    }
}
