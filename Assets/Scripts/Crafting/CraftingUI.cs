using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CraftingUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TextMeshProUGUI crafterNameText;
    [SerializeField] private Transform recipeListContainer;
    [SerializeField] private GameObject recipeSlotPrefab;
    [SerializeField] private Button closeButton;

    [Header("레시피 상세")]
    [SerializeField] private GameObject recipeDetailPanel;
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Image resultIconImage;
    [SerializeField] private TextMeshProUGUI ingredientsText;
    [SerializeField] private Button craftButton;

    [Header("시스템")]
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private Inventory inventory;

    private List<RecipeSlotUI> recipeSlots = new List<RecipeSlotUI>();
    private RecipeData selectedRecipe;

    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => craftingSystem.CloseCrafting());
        }

        if (craftButton != null)
        {
            craftButton.onClick.AddListener(OnCraftButtonClicked);
        }

        if (recipeDetailPanel != null)
        {
            recipeDetailPanel.SetActive(false);
        }
    }

    public void SetCrafter(Crafter crafter)
    {
        if (crafterNameText != null)
        {
            crafterNameText.text = crafter.CrafterName;
        }
    }

    public void DisplayRecipes(RecipeData[] recipes)
    {
        selectedRecipe = null;

        if (recipeDetailPanel != null)
        {
            recipeDetailPanel.SetActive(false);
        }

        foreach (var slot in recipeSlots)
        {
            Destroy(slot.gameObject);
        }
        recipeSlots.Clear();

        foreach (var recipe in recipes)
        {
            GameObject slotObj = Instantiate(recipeSlotPrefab, recipeListContainer);
            RecipeSlotUI slotUI = slotObj.GetComponent<RecipeSlotUI>();

            if (slotUI != null)
            {
                slotUI.SetRecipe(recipe, inventory);
                slotUI.OnRecipeSelected += SelectRecipe;
                recipeSlots.Add(slotUI);
            }
        }
    }

    void SelectRecipe(RecipeData recipe)
    {
        selectedRecipe = recipe;

        if (recipeDetailPanel != null)
        {
            recipeDetailPanel.SetActive(true);
        }

        if (recipeNameText != null)
        {
            recipeNameText.text = recipe.RecipeName;
        }

        if (resultIconImage != null && recipe.resultItem != null)
        {
            resultIconImage.sprite = recipe.resultItem.icon;
            resultIconImage.enabled = true;
        }

        if (ingredientsText != null)
        {
            ingredientsText.text = recipe.GetRecipeDescription();
        }

        UpdateCraftButton();
    }

    void UpdateCraftButton()
    {
        if (craftButton == null || selectedRecipe == null)
            return;

        bool canCraft = craftingSystem.CanCraft(selectedRecipe);
        craftButton.interactable = canCraft;
    }

    void OnCraftButtonClicked()
    {
        if (selectedRecipe != null)
        {
            craftingSystem.TryCraft(selectedRecipe);

            UpdateCraftButton();
            RefreshRecipeList();
        }
    }

    void RefreshRecipeList()
    {
        foreach (var slot in recipeSlots)
        {
            slot.Refresh(inventory);
        }
    }
}