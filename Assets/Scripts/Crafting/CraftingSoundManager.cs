using UnityEngine;

public class CraftingSoundManager : MonoBehaviour
{
    [Header("사운드")]
    [SerializeField] private AudioClip craftingStartSound;
    [SerializeField] private AudioClip craftingSuccessSound;
    [SerializeField] private AudioClip craftingFailSound;
    [SerializeField] private AudioSource audioSource;

    [Header("시스템")]
    [SerializeField] private CraftingSystem craftingSystem;

    void Start()
    {
        if (craftingSystem != null)
        {
            craftingSystem.OnCraftingStarted += PlayCraftingStart;
            craftingSystem.OnCraftingCompleted += PlayCraftingComplete;
        }
    }

    void OnDestroy()
    {
        if (craftingSystem != null)
        {
            craftingSystem.OnCraftingStarted -= PlayCraftingStart;
            craftingSystem.OnCraftingCompleted -= PlayCraftingComplete;
        }
    }

    void PlayCraftingStart(RecipeData recipe)
    {
        if (audioSource != null && craftingStartSound != null)
        {
            audioSource.PlayOneShot(craftingStartSound);
        }
    }

    void PlayCraftingComplete(RecipeData recipe, bool success)
    {
        if (audioSource == null)
            return;

        if (success && craftingSuccessSound != null)
        {
            audioSource.PlayOneShot(craftingSuccessSound);
        }
        else if (!success && craftingFailSound != null)
        {
            audioSource.PlayOneShot(craftingFailSound);
        }
    }
}