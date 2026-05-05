using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    [Header("ÂüÁ¶")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject inventoryPanel;

    [Header("ÅÇ ¹öÆ°")]
    [SerializeField] private Button tabConsumables;
    [SerializeField] private Button tabEquipment;
    [SerializeField] private Button tabMaterials;
    [SerializeField] private Button tabSpecial;

    [Header("±×¸®µå")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;

    [Header("ÅøÆÁ")]
    [SerializeField] private ItemTooltip tooltip;

    private DisplayTab currentTab = DisplayTab.Consumables;
    private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();

    void Awake()
    {
        
        CreateSlotUIs(24);
    }

    void Start()
    {
        tabConsumables.onClick.AddListener(() => ShowTab(DisplayTab.Consumables));
        tabEquipment.onClick.AddListener(() => ShowTab(DisplayTab.Equipment));
        tabMaterials.onClick.AddListener(() => ShowTab(DisplayTab.Materials));
        tabSpecial.onClick.AddListener(() => ShowTab(DisplayTab.Special));

        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshUI;
        }

        inventoryPanel.SetActive(true);

        if (tooltip != null)
        {
            tooltip.Hide();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void CreateSlotUIs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();

            int index = i;
            slotUI.OnSlotClicked += () => OnSlotClicked(index);
            slotUI.OnSlotHoverEnter += (slot) => OnSlotHoverEnter(slot);
            slotUI.OnSlotHoverExit += () => OnSlotHoverExit();

            slotUIs.Add(slotUI);
        }
    }

    void ToggleInventory()
    {
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
        {
            RefreshUI();
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            if (tooltip != null)
                tooltip.Hide();
        }
    }

    void ShowTab(DisplayTab tab)
    {
        currentTab = tab;
        UpdateTabButtons();
        RefreshUI();
    }

    void UpdateTabButtons()
    {
        tabConsumables.interactable = currentTab != DisplayTab.Consumables;
        tabEquipment.interactable = currentTab != DisplayTab.Equipment;
        tabMaterials.interactable = currentTab != DisplayTab.Materials;
        tabSpecial.interactable = currentTab != DisplayTab.Special;
    }

    void RefreshUI()
    {
        var allItems = inventory.GetAllItems();
        var filteredItems = allItems.Where(slot =>
            slot.item.GetDisplayTab() == currentTab
        ).ToList();

        foreach (var slotUI in slotUIs)
        {
            slotUI.Clear();
        }

        for (int i = 0; i < filteredItems.Count && i < slotUIs.Count; i++)
        {
            slotUIs[i].SetSlot(filteredItems[i]);
        }
    }

    void OnSlotClicked(int slotIndex)
    {
        var allItems = inventory.GetAllItems();
        var filteredItems = allItems.Where(slot =>
            slot.item.GetDisplayTab() == currentTab
        ).ToList();

        if (slotIndex >= filteredItems.Count)
            return;

        var slot = filteredItems[slotIndex];
        int realIndex = inventory.GetSlotIndex(slot);

        if (realIndex < 0)
            return;

        if (slot.item.category == ItemCategory.Consumable)
        {
            inventory.UseItem(realIndex);
        }
        else if (IsEquipment(slot.item.category))
        {
            inventory.EquipItem(realIndex);
        }
    }

    void OnSlotHoverEnter(InventorySlot slot)
    {
        if (tooltip != null && slot != null && !slot.IsEmpty)
        {
            tooltip.Show(slot.item);
        }
    }

    void OnSlotHoverExit()
    {
        if (tooltip != null)
        {
            tooltip.Hide();
        }
    }

    bool IsEquipment(ItemCategory category)
    {
        return category == ItemCategory.Weapon ||
               category == ItemCategory.Armor ||
               category == ItemCategory.Accessory;
    }
}