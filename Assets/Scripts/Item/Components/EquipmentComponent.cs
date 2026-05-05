using UnityEngine;

public abstract class EquipmentComponent : ItemComponent
{
    [Header("장비 기본 정보")]
    public EquipmentSlot slot;

    public abstract void OnEquip(GameObject target);

    public abstract void OnUnequip(GameObject target);

    public virtual bool CanEquip(GameObject target)
    {
        return true;
    }
}