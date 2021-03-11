using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Modules
{
    public enum ModuleUIInfo
    {
        EnergyConsumption,
        FuelConsumption,
        Force,
        Torque,
        Power,
        Storage,
        WeaponType,
        Damage,
        ReloadTime
    }

    public class ModuleUI : MonoBehaviour
    {
        private const int infoTypesCount = 9;

        private static readonly string[] formatStrings = new[]
        {
            "Энергопотребление",
            "Расход топлива",
            "Тяга",
            "Момент силы",
            "Мощность",
            "Ёмкость",
            "Тип",
            "Урон",
            "Перезарядка"
        };

        [SerializeField]
        private TMP_Text infoText;
        [SerializeField]
        private LayoutElement layout;

        public void UpdateInfo(Module module)
        {
            string info = module.ToString();
            for (int i = 0; i < infoTypesCount; i++)
            {
                string head = ((ModuleUIInfo)i).ToString();
                if (info.Contains(head))
                    info = info.Replace(head, formatStrings[i]);
            }
            infoText.text = info;
            layout.minHeight = infoText.preferredHeight;
        }
    }
}
