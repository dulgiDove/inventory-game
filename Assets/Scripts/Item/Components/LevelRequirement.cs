using UnityEngine;

[CreateAssetMenu(fileName = "Level Requirement", menuName = "Inventory/Components/Requirements/Level")]
public class LevelRequirement : RequirementComponent
{
    [Header("레벨 제한")]
    public int requiredLevel = 1;

    public override bool IsMet(GameObject target)
    {
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats == null)
            return false;

        return stats.level >= requiredLevel;
    }

    public override string GetComponentName() => "레벨 제한";

    public override string GetDescription()
    {
        return $"요구 레벨: {requiredLevel}";
    }
}