using Game.Voxels.Editor.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Voxels.Editor
{
    public class PlayerInventory : MonoBehaviour, IGameData
    {
        private PlayerController controller;

        [SerializeField]
        private VoxelItemUI itemSlotPrefab;

        [SerializeField]
        private RectTransform inventoryContent;
        [SerializeField]
        private VoxelItemUI[] hotbarSlots;

        private Inventory hotbar;
        private Inventory inventory;

        [SerializeField]
        private RectTransform selectedHotbarFrame;
        private int selectedHotbarItemIndex = 0;

        public VoxelItem selectedItem => hotbar.items[selectedHotbarItemIndex];

        [SerializeField]
        private RectTransform selectedInventoryFrame;
        private int selectedInventoryItemIndex = -1;

        private bool choseInventory;

        [SerializeField]
        private VoxelRenderer2D voxelRenderer;
        [SerializeField]
        private SplashMessage hotbarSplashMessage;

        private void Awake()
        {
            hotbar.itemSlots = new List<VoxelItemUI>(hotbarSlots);

            for (int i = hotbar.items.Count; i < hotbar.itemSlots.Count; i++)
                hotbar.items.Add(new VoxelItem());
            for (int i = 0; i < hotbar.itemSlots.Count; i++)
            {
                voxelRenderer.CreateVoxelTexture(hotbar.itemSlots[i].icon);
                int index = i;
                hotbar.itemSlots[i].button.onClick.AddListener(() => SwitchHotbarItem(index));
            }

            inventory.itemSlots = new List<VoxelItemUI>();
            for (int i = 0; i < inventory.items.Count; i++)
            {
                VoxelItemUI slot = Instantiate(itemSlotPrefab, inventoryContent);
                inventory.itemSlots.Add(slot);

                voxelRenderer.CreateVoxelTexture(inventory.itemSlots[i].icon);
                int index = i;
                slot.button.onClick.AddListener(() => SwitchInventoryItem(index));
            }

            controller = GetComponent<PlayerController>();
        }

        private void Start()
        {
            UpdateInventoryUI(hotbar);
            UpdateInventoryUI(inventory);
            selectedHotbarFrame.position = hotbar.itemSlots[selectedHotbarItemIndex].transform.position;
        }

        private void OnEnable()
        {
            controller.onVoxelPlace.AddListener(OnVoxelPlace);
        }

        private void OnDisable()
        {
            controller.onVoxelPlace.RemoveListener(OnVoxelPlace);
        }

        public void OnLoad(string json)
        {
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            hotbar = data.hotbar;
            inventory = data.inventory;
            selectedHotbarItemIndex = data.selectedHotbarIndex;
            selectedInventoryItemIndex = data.selectedInventoryIndex;
        }

        public string OnSave()
        {
            return JsonUtility.ToJson(new InventoryData()
            {
                hotbar = hotbar,
                inventory = inventory,
                selectedHotbarIndex = selectedHotbarItemIndex,
                selectedInventoryIndex = selectedInventoryItemIndex
            });
        }

        public void SwitchHotbarItemByInput(float value)
        {
            selectedHotbarItemIndex = (selectedHotbarItemIndex - (int)Mathf.Sign(value) + hotbar.itemSlots.Count) % hotbar.itemSlots.Count;
            selectedHotbarFrame.position = hotbar.itemSlots[selectedHotbarItemIndex].transform.position;

            hotbarSplashMessage.SetActive(((Modules.Material)(hotbar.items[selectedHotbarItemIndex].content & 0x0F)).ToString());
        }

        private void SwitchHotbarItem(int index)
        {
            if (choseInventory && selectedInventoryItemIndex != -1)
            {
                if (hotbar.items[index].content == 0)
                {
                    hotbar.items[index] = inventory.items[selectedInventoryItemIndex];
                    inventory.items.RemoveAt(selectedInventoryItemIndex);

                    SwitchInventoryItem(selectedInventoryItemIndex);
                }
                else
                    SwapItems(inventory, selectedInventoryItemIndex, hotbar, index);
                UpdateInventoryUI(inventory);
            }
            else if (inventoryContent.gameObject.activeInHierarchy)
                SwapItems(hotbar, selectedHotbarItemIndex, hotbar, index);
            else
                hotbarSplashMessage.SetActive(((Modules.Material)(hotbar.items[index].content & 0x0F)).ToString());

            selectedHotbarFrame.position = hotbar.itemSlots[index].transform.position;
            selectedHotbarItemIndex = index;
            choseInventory = false;
            UpdateInventoryUI(hotbar);
        }

        private void SwapItems(Inventory fromInventory, int fromIndex, Inventory toInventory, int toIndex)
        {
            VoxelItem tItem = fromInventory.items[fromIndex];
            fromInventory.items[fromIndex] = toInventory.items[toIndex];
            toInventory.items[toIndex] = tItem;
        }

        private void SwitchInventoryItem(int index)
        {
            if (index == selectedInventoryItemIndex)
            {
                selectedInventoryItemIndex = -1;
                selectedInventoryFrame.gameObject.SetActive(false);
            }
            else
            {
                selectedInventoryItemIndex = index;
                selectedInventoryFrame.gameObject.SetActive(true);
                selectedInventoryFrame.position = inventory.itemSlots[index].transform.position;
                selectedInventoryFrame.SetParent(inventory.itemSlots[index].transform);
            }
            choseInventory = true;
        }

        private void UpdateInventoryUI(Inventory inventory)
        {
            int i = 0;
            for (; i < inventory.items.Count; i++)
            {
                inventory.itemSlots[i].amount.text = inventory.items[i].amount.ToString();
                inventory.itemSlots[i].health.text = inventory.items[i].health.ToString();
                voxelRenderer.UpdateVoxelTexture(inventory.items[i], inventory.itemSlots[i].icon);
                inventory.itemSlots[i].amount.gameObject.SetActive(inventory.items[i].content != 0);
                inventory.itemSlots[i].health.gameObject.SetActive(inventory.items[i].content != 0);
                inventory.itemSlots[i].icon.  gameObject.SetActive(inventory.items[i].content != 0);
            }
            for (i = inventory.itemSlots.Count - 1; i > inventory.items.Count - 1; i--)
            {
                Destroy(inventory.itemSlots[i].gameObject);
                inventory.itemSlots.RemoveAt(i);
            }
        }

        private void OnVoxelPlace(VoxelPlaceEventArgs args)
        {
            if (args.selectedItem.content == 0)
                args.handleCode = VoxelPlaceEventArgs.HandleCode.EmptyVoxelPlace;
        }
    }
}
