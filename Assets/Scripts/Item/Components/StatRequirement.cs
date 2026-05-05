using UnityEngine;

[CreateAssetMenu(fileName = "Stat Requirement", menuName = "Inventory/Components/Requirements/Stat")]
public class StatRequirement : RequirementComponent
{
    public enum StatType
    {
        Strength,
        Dexterity,
        Intelligence,
        Vitality
    }

    [Header("Ω∫≈» ¡¶«—")]
    public StatType statType;
    public int requiredValue;

    public override bool IsMet(GameObject target)
    {
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats == null)
            return false;

        switch (statType)
        {
            case StatType.Strength:
                return stats.strength >= requiredValue;
            case StatType.Dexterity:
                return stats.dexterity >= requiredValue;
            case StatType.Intelligence:
                return stats.intelligence >= requiredValue;
            case StatType.Vitality:
                return stats.vitality >= requiredValue;
        }

        return false;
    }

    public override string GetComponentName() => "Ω∫≈» ¡¶«—";

    public override string GetDescription()
    {
        return $"ø‰±∏ {statType}: {requiredValue}";
    }
}
