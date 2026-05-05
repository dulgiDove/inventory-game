using UnityEngine;

[CreateAssetMenu(fileName = "Class Requirement", menuName = "Inventory/Components/Requirements/Class")]
public class ClassRequirement : RequirementComponent
{
    [Header("流诀 力茄")]
    public CharacterClass[] allowedClasses;

    public override bool IsMet(GameObject target)
    {
        PlayerClass playerClass = target.GetComponent<PlayerClass>();
        if (playerClass == null)
            return false;

        foreach (var allowed in allowedClasses)
        {
            if (playerClass.currentClass == allowed)
                return true;
        }

        return false;
    }

    public override string GetComponentName() => "流诀 力茄";

    public override string GetDescription()
    {
        string classList = "";
        for (int i = 0; i < allowedClasses.Length; i++)
        {
            classList += allowedClasses[i].ToString();
            if (i < allowedClasses.Length - 1)
                classList += ", ";
        }

        return $"流诀 力茄: {classList}";
    }
}