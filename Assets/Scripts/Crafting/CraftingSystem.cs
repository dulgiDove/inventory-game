using UnityEngine;
using System.Linq;

public class CraftingSystem : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private RecipeDatabase recipeDatabase;
    [SerializeField] private CraftingUI craftingUI;

    [Header("현재 상태")]
    private Crafter currentCrafter;
    private RecipeData[] currentRecipes;

    public bool IsOpen { get; private set; }

    public event System.Action<RecipeData> OnCraftingStarted;
    public event System.Action<RecipeData, bool> OnCraftingCompleted;
    public event System.Action<CrafterType> OnCraftingUIOpened;
    public event System.Action OnCraftingUIClosed;

    void Start()
    {
        if (craftingUI != null)
        {
            craftingUI.gameObject.SetActive(false);
        }
    }

    public void OpenCrafting(Crafter crafter)
    {
        if (crafter == null || recipeDatabase == null)
            return;

        currentCrafter = crafter;
        currentRecipes = recipeDatabase.GetRecipesByType(crafter.CrafterType);

        IsOpen = true;

        if (craftingUI != null)
        {
            craftingUI.gameObject.SetActive(true);
            craftingUI.SetCrafter(crafter);
            craftingUI.DisplayRecipes(currentRecipes);
        }

        OnCraftingUIOpened?.Invoke(crafter.CrafterType);

        Time.timeScale = 0f;

        Debug.Log($"{crafter.CrafterName} 제작 UI 열림");
    }

    public void CloseCrafting()
    {
        IsOpen = false;

        if (craftingUI != null)
        {
            craftingUI.gameObject.SetActive(false);
        }

        currentCrafter = null;
        currentRecipes = null;

        OnCraftingUIClosed?.Invoke();

        Time.timeScale = 1f;

        Debug.Log("제작 UI 닫힘");
    }

    public void TryCraft(RecipeData recipe)
    {
        if (recipe == null)
            return;

        OnCraftingStarted?.Invoke(recipe);

        if (!CanCraft(recipe))
        {
            Debug.Log("재료가 부족합니다!");
            OnCraftingCompleted?.Invoke(recipe, false);
            return;
        }

        foreach (var ingredient in recipe.ingredients)
        {
            inventory.RemoveItem(ingredient.item, ingredient.amount);
        }

        bool success = inventory.AddItem(recipe.resultItem, recipe.resultAmount);

        if (success)
        {
            Debug.Log($"{recipe.resultItem.itemName} x{recipe.resultAmount} 제작 완료!");

            OnCraftingCompleted?.Invoke(recipe, true);
        }
        else
        {
            Debug.LogWarning("인벤토리가 꽉 찼습니다!");

            foreach (var ingredient in recipe.ingredients)
            {
                inventory.AddItem(ingredient.item, ingredient.amount);
            }

            OnCraftingCompleted?.Invoke(recipe, false);
        }
    }

    public bool CanCraft(RecipeData recipe)
    {
        if (recipe == null || recipe.ingredients == null)
            return false;

        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.item == null)
                continue;

            var slots = inventory.GetAllItems();
            int totalCount = slots
                .Where(s => !s.IsEmpty && s.item == ingredient.item)
                .Sum(s => s.count);

            if (totalCount < ingredient.amount)
                return false;
        }

        return true;
    }

    void Update()
    {
        if (IsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCrafting();
        }
    }
}