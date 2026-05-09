using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSlotUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image resultIcon;
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private TextMeshProUGUI ingredientsText;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject cantCraftOverlay;

    private CraftingSystem craftingSystem;
    private RecipeData recipe;

    public event System.Action<RecipeData> OnRecipeSelected;

    void Start()
    {
        if (selectButton != null)
        {
            selectButton.onClick.AddListener(() => OnRecipeSelected?.Invoke(recipe));
        }
    }

    public void SetRecipe(RecipeData recipeData, Inventory inventory, CraftingSystem craftingSystem)
    {
        this.craftingSystem = craftingSystem;
        recipe = recipeData;

        if (resultIcon != null && recipe.resultItem != null)
            resultIcon.sprite = recipe.resultItem.icon;

        if (recipeNameText != null)
            recipeNameText.text = recipe.RecipeName;

        if (ingredientsText != null)
            ingredientsText.text = recipe.GetRecipeDescription();

        Refresh(inventory);
    }

    public void Refresh(Inventory inventory)
    {
        bool canCraft = craftingSystem.CanCraft(recipe);

        if (cantCraftOverlay != null)
            cantCraftOverlay.SetActive(!canCraft);

        if (resultIcon != null)
            resultIcon.color = canCraft ? Color.white : new Color(1, 1, 1, 0.5f);
    }
}