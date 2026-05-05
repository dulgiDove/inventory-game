using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Crafting/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    [Header("모든 레시피")]
    public RecipeData[] recipes;

    public RecipeData[] GetRecipesByType(CrafterType crafterType)
    {
        return recipes.Where(r => r.crafterType == crafterType).ToArray();
    }

    public RecipeData GetRecipeByResult(ItemData resultItem)
    {
        return recipes.FirstOrDefault(r => r.resultItem == resultItem);
    }
}