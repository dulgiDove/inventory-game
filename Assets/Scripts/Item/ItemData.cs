using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemID;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("분류")]
    public ItemCategory category;

    [Header("스택")]
    public bool isStackable = true;
    public int maxStack = 99;

    [Header("가격")]
    public int buyPrice;
    public int sellPrice;

    [Header("컴포넌트들")]
    public ItemComponent[] components;

    // 특정 타입의 컴포넌트 가져오기
    public T GetComponent<T>() where T : ItemComponent
    {
        foreach (var comp in components)
        {
            if (comp is T)
                return comp as T;
        }
        return null;
    }

    // 특정 타입의 모든 컴포넌트 가져오기
    public T[] GetComponents<T>() where T : ItemComponent
    {
        return components.OfType<T>().ToArray();
    }

    // 인벤토리 어느 탭에 표시될지 선택
    public DisplayTab GetDisplayTab()
    {
        switch (category)
        {
            case ItemCategory.Consumable:
                return DisplayTab.Consumables;

            case ItemCategory.Weapon:
            case ItemCategory.Armor:
            case ItemCategory.Accessory:
                return DisplayTab.Equipment;

            case ItemCategory.Material:
                return DisplayTab.Materials;

            case ItemCategory.QuestItem:
            case ItemCategory.KeyItem:
            case ItemCategory.Currency:
                return DisplayTab.Special;

            default:
                return DisplayTab.Misc;
        }
    }

    // 착용 가능한지
    public bool CanEquip(GameObject target)
    {
        var requirements = GetComponents<RequirementComponent>();

        foreach (var req in requirements)
        {
            if (!req.IsMet(target))
            {
                Debug.Log($"조건 미충족: {req.GetDescription()}");
                return false;
            }
        }

        return true;
    }

    // 착용
    public void Equip(GameObject target)
    {
        var equipment = GetComponent<EquipmentComponent>();
        if (equipment != null && CanEquip(target))
        {
            equipment.OnEquip(target);
        }
    }

    // 착용 해제
    public void Unequip(GameObject target)
    {
        var equipment = GetComponent<EquipmentComponent>();
        if (equipment != null)
        {
            equipment.OnUnequip(target);
        }
    }

    // 사용
    public void Use(GameObject target)
    {
        var effects = GetComponents<ItemEffect>();
        foreach (var effect in effects)
        {
            effect.Apply(target);
        }
    }

    // 전체 설명
    public string GetFullDescription()
    {
        string result = description + "\n\n";

        foreach (var comp in components)
        {
            result += $"[{comp.GetComponentName()}]\n";
            result += comp.GetDescription() + "\n\n";
        }

        return result;
    }
}