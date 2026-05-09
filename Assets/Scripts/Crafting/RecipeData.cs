using UnityEngine;
using System.Linq;

public enum CrafterType
{
    Alchemist,
    Blacksmith,
    SecretMerchant
}

[System.Serializable]
public class Ingredient
{
    public ItemData item;
    public int amount;
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class RecipeData : ScriptableObject
{
    [Header("결과물")]
    public ItemData resultItem;
    public int resultAmount = 1;

    [Header("제작 분류")]
    public CrafterType crafterType;

    [Header("재료")]
    public Ingredient[] ingredients;

    public string RecipeName => resultItem != null ? resultItem.itemName : "Unknown";

    public string GetRecipeDescription()
    {
        if (ingredients == null || ingredients.Length == 0)
            return "재료 없음";

        var lines = System.Linq.Enumerable
            .Where(ingredients, i => i.item != null)
            .Select(i => $"{i.item.itemName} x{i.amount}");

        return string.Join("\n", lines);
    }
}