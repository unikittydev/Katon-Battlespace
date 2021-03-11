using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Modules
{
    [RequireComponent(typeof(Ship))]
    public class ShipModulesUI : MonoBehaviour
    {
        [SerializeField]
        private Transform moduleGroupPrefab;
        [SerializeField]
        private ModuleUI moduleUIPrefab;

        [SerializeField]
        private Transform content;

        private Dictionary<Type, Transform> moduleGroups;

        private Ship ship;

        private Dictionary<Module, ModuleUI> moduleInfo = new Dictionary<Module, ModuleUI>();

        private void Start()
        {
            ship = GetComponent<Ship>();
            ship.modules.onAddModule += OnAddModule;
            ship.modules.onRemoveModule += OnRemoveModule;

            moduleGroups = new Dictionary<Type, Transform>();

            for (int i = 0; i < Module.moduleTypes.Length; i++)
            {
                Transform moduleGroup = Instantiate(moduleGroupPrefab, content);
                moduleGroup.name = Module.moduleTypes[i].Name;
                moduleGroup.GetChild(0).GetComponent<TMP_Text>().text = Module.moduleTypes[i].Name;
                moduleGroups.Add(Module.moduleTypes[i], moduleGroup);
            }
        }

        private void OnDisable()
        {
            ship.modules.onAddModule -= OnAddModule;
            ship.modules.onRemoveModule -= OnRemoveModule;
        }

        private void OnAddModule(Module module)
        {
            ModuleUI newModuleUI = Instantiate(moduleUIPrefab, content);
            newModuleUI.transform.SetSiblingIndex(moduleGroups[module.GetType()].GetSiblingIndex() + 1);
            moduleInfo.Add(module, newModuleUI);
            newModuleUI.UpdateInfo(module);
        }

        private void OnRemoveModule(Module module)
        {
            Destroy(moduleInfo[module].gameObject);
            moduleInfo.Remove(module);
        }
    }
}
