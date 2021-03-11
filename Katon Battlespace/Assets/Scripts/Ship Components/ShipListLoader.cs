using Game.Voxels.Editor;
using Game.Voxels.Editor.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ShipListLoader : MonoBehaviour
    {
        [SerializeField]
        private ShipSlotUI shipSlotPrefab;
        [SerializeField]
        private Transform shipSlotsContent;
        [SerializeField]
        private TMP_InputField shipName;

        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button loadButton;

        private ShipSlotUI selectedSlot;

        public string loadShipName => selectedSlot.shipName.text;

        private string _saveShipName;
        public string saveShipName => _saveShipName;

        private void Awake()
        {
            LoadShips();

            shipName.onValueChanged.AddListener(value =>
            {
                _saveShipName = value;
                if (saveButton != null)
                    saveButton.interactable = !string.IsNullOrEmpty(value.Trim());
            });
        }

        public void LoadShips()
        {
            for (int i = 0; i < shipSlotsContent.childCount; i++)
                Destroy(shipSlotsContent.GetChild(i).gameObject);
            OnSlotSelect(null);

            string[] jsonArray = GameDataManager.LoadAll(typeof(VoxelWorldEditor));
            ShipStatsData data;

            for (int i = 0; i < jsonArray.Length; i++)
            {
                data = JsonUtility.FromJson<ShipStatsData>(jsonArray[i]);
                ShipSlotUI shipSlot = Instantiate(shipSlotPrefab, shipSlotsContent);
                shipSlot.shipName.SetText(data.name);
                shipSlot.mass.SetText(data.voxelCount.ToString());
                shipSlot.health.SetText(data.totalHealth.ToString());
                shipSlot.selectionButton.onClick.AddListener(() => OnSlotSelect(shipSlot));
            }
        }

        private void OnSlotSelect(ShipSlotUI slot)
        {
            if (selectedSlot != null)
                selectedSlot.selectionImage.color = new Color(0f, 0f, 0f, 0f);
            if (slot != null)
            {
                slot.selectionImage.color = new Color(0f, 0f, 0f, .25f);
                shipName.text = slot.shipName.text;
                if (saveButton != null)
                    saveButton.interactable = true;
            }
            if (loadButton != null)
                loadButton.interactable = slot != null;
            selectedSlot = slot;
        }
    }
}
