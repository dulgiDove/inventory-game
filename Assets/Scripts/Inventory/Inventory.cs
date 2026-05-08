using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private int maxSlots = 24;

    private List<InventorySlot> slots;

    public event Action OnInventoryChanged;
    public event Action<ItemCategory> OnItemUsed;

    void Awake()
    {
        InitializeSlots();
    }

    void InitializeSlots()
    {
        if (slots == null)
        {
            slots = new List<InventorySlot>();
            for (int i = 0; i < maxSlots; i++)
            {
                slots.Add(new InventorySlot(null, 0));
            }
            Debug.Log($"인벤토리 초기화: {maxSlots}개 슬롯");
        }
    }

    // 아이템 추가
    public bool AddItem(ItemData item, int count = 1)
    {
        if (slots == null)
        {
            Debug.LogError("인벤토리가 초기화되지 않았습니다!");
            InitializeSlots();
        }

        if (item == null)
        {
            Debug.LogError("아이템이 null입니다!");
            return false;
        }

        int remaining = count;

        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.count < item.maxStack)
                {
                    remaining = slot.AddItem(item, remaining);
                    if (remaining <= 0) break;
                }
            }
        }

        while (remaining > 0)
        {
            var emptySlot = slots.FirstOrDefault(s => s.IsEmpty);
            if (emptySlot == null)
            {
                Debug.Log("인벤토리가 가득 찼습니다!");
                OnInventoryChanged?.Invoke();
                return false;
            }

            remaining = emptySlot.AddItem(item, remaining);
        }

        Debug.Log($"{item.itemName} x{count} 획득!");
        OnInventoryChanged?.Invoke();
        return true;
    }

    // 아이템 제거
    public bool RemoveItem(ItemData item, int count = 1)
    {
        int remaining = count;

        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                if (slot.count >= remaining)
                {
                    slot.count -= remaining;
                    if (slot.count == 0)
                        slot.Clear();

                    OnInventoryChanged?.Invoke();
                    return true;
                }
                else
                {
                    remaining -= slot.count;
                    slot.Clear();
                }
            }
        }

        OnInventoryChanged?.Invoke();
        return remaining == 0;
    }

    // 아이템 사용
    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
            return;

        var slot = slots[slotIndex];
        if (slot.IsEmpty)
            return;

        var item = slot.item;

        OnItemUsed?.Invoke(item.category);

        if (item.category == ItemCategory.Consumable)
        {
            item.Use(gameObject);

            slot.count--;
            if (slot.count <= 0)
                slot.Clear();

            OnInventoryChanged?.Invoke();
        }
    }

    // 장비 착용
    public bool EquipItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
            return false;

        var slot = slots[slotIndex];
        if (slot.IsEmpty)
            return false;

        var item = slot.item;
        var equipment = item.GetComponent<EquipmentComponent>();

        if (equipment == null)
        {
            Debug.Log("장비가 아닙니다!");
            return false;
        }

        if (!item.CanEquip(gameObject))
        {
            Debug.Log("착용 조건을 만족하지 않습니다!");
            return false;
        }

        item.Equip(gameObject);
        return true;
    }

    // 전체 아이템
    public List<InventorySlot> GetAllItems()
    {
        return slots.Where(s => !s.IsEmpty).ToList();
    }

    // 슬롯 가져오기
    public InventorySlot GetSlot(int index)
    {
        if (index >= 0 && index < slots.Count)
            return slots[index];
        return null;
    }

    // 슬롯 인덱스 찾기
    public int GetSlotIndex(InventorySlot targetSlot)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == targetSlot)
                return i;
        }
        return -1;
    }

    // 아이템 개수 확인
    public int GetItemCount(ItemData item)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (slot.item == item)
                total += slot.count;
        }
        return total;
    }

    // 아이템 보유 여부
    public bool HasItem(ItemData item, int count = 1)
    {
        return GetItemCount(item) >= count;
    }
}