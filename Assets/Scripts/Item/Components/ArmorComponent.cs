using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Inventory/Components/Equipment/Armor")]
public class ArmorComponent : EquipmentComponent
{
    [Header("방어구 정보")]
    public int defense = 20;
    public int health = 50;

    public override void OnEquip(GameObject target)
    {
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.AddDefense(defense);
            stats.AddMaxHealth(health);

            Debug.Log($"방어구 장착: 방어력 +{defense}, 체력 +{health}");
        }
    }

    public override void OnUnequip(GameObject target)
    {
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.RemoveDefense(defense);
            stats.RemoveMaxHealth(health);

            Debug.Log("방어구 해제");
        }
    }

    public override string GetComponentName() => "방어구";

    public override string GetDescription()
    {
        return $"방어력 +{defense}\n최대 체력 +{health}";
    }
}