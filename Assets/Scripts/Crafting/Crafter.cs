using UnityEngine;

public class Crafter : MonoBehaviour, IInteractable
{
    [Header("제작자 정보")]
    [SerializeField] private CrafterType crafterType;
    [SerializeField] private string crafterName = "연금술사";

    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;

    [Header("시스템 참조")]
    [SerializeField] private CraftingSystem craftingSystem;

    private bool playerInRange = false;

    public CrafterType CrafterType => crafterType;
    public string CrafterName => crafterName;

    void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (craftingSystem != null)
        {
            craftingSystem.OpenCrafting(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }

            if (craftingSystem != null && craftingSystem.IsOpen)
            {
                craftingSystem.CloseCrafting();
            }
        }
    }
}