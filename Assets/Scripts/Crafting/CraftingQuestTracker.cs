using UnityEngine;

public class CraftingQuestTracker : MonoBehaviour
{
    [SerializeField] private CraftingSystem craftingSystem;
    // [SerializeField] private QuestSystem questSystem; // 나중에 추가

    void Start()
    {
        if (craftingSystem != null)
        {
            craftingSystem.OnCraftingCompleted += OnItemCrafted;
        }
    }

    void OnDestroy()
    {
        if (craftingSystem != null)
        {
            craftingSystem.OnCraftingCompleted -= OnItemCrafted;
        }
    }

    void OnItemCrafted(RecipeData recipe, bool success)
    {
        if (!success)
            return;

        Debug.Log($"퀘스트 체크: {recipe.resultItem.itemName} 제작됨");

        // 나중에 퀘스트 시스템 연동
        // questSystem.CheckCraftingQuest(recipe.resultItem);
    }
}