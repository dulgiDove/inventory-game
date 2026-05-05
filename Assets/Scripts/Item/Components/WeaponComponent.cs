using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Components/Equipment/Weapon")]
public class WeaponComponent : EquipmentComponent
{
    [Header("무기 정보")]
    public int attackPower = 10;
    public float attackSpeed = 1f;
    public float attackRange = 2f;

    [Header("특수 효과")]
    public float criticalChance = 0.1f;
    public float criticalDamage = 1.5f;

    public override void OnEquip(GameObject target)
    {
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.AddAttackPower(attackPower);
            stats.SetAttackSpeed(attackSpeed);
            stats.SetAttackRange(attackRange);

            Debug.Log($"무기 장착: 공격력 +{attackPower}");
        }
    }
    public override void OnUnequip(GameObject target)
    {
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.RemoveAttackPower(attackPower);
            stats.ResetAttackSpeed();
            stats.ResetAttackRange();

            Debug.Log("무기 해제");
        }
    }

    public override string GetComponentName() => "무기";

    public override string GetDescription()
    {
        return $"공격력 +{attackPower}\n공격속도: {attackSpeed}\n사거리: {attackRange}";
    }
}