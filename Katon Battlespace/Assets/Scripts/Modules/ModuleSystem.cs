using UnityEngine;
using UnityEngine.Events;

namespace Game.Modules
{
    public class ModuleSystem : MonoBehaviour
    {
        private static readonly UnityEvent updateMethods = new UnityEvent();
        private static readonly UnityEvent fixedUpdateMethods = new UnityEvent();

        private void Update()
        {
            updateMethods.Invoke();
        }

        private void FixedUpdate()
        {
            fixedUpdateMethods.Invoke();
        }

        public static void AddUpdateModule(UnityAction method) => updateMethods.AddListener(method);

        public static void AddFixedUpdateModule(UnityAction method) => fixedUpdateMethods.AddListener(method);

        public static void RemoveUpdateModule(UnityAction method) => updateMethods.RemoveListener(method);

        public static void RemoveFixedUpdateModule(UnityAction method) => fixedUpdateMethods.RemoveListener(method);
    }
}
