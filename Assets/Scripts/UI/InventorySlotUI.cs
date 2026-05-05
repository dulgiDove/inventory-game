using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private GameObject emptyIndicator;

    private InventorySlot slot;

    public event System.Action OnSlotClicked;
    public event System.Action<InventorySlot> OnSlotHoverEnter;
    public event System.Action OnSlotHoverExit;

    public void SetSlot(InventorySlot inventorySlot)
    {
        slot = inventorySlot;

        if (slot == null || slot.IsEmpty)
        {
            Clear();
            return;
        }

        if (iconImage != null)
        {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = slot.item.icon;
            iconImage.enabled = true;
            iconImage.color = Color.white;
        }

        if (countText != null)
        {
            countText.gameObject.SetActive(true);

            if (slot.item.isStackable && slot.count > 1)
            {
                countText.text = slot.count.ToString();
                countText.enabled = true;
            }
            else
            {
                countText.enabled = false;
            }
        }

        if (emptyIndicator != null)
            emptyIndicator.SetActive(false);
    }

    public void Clear()
    {
        slot = null;
        if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }

        if (countText != null)
        {
            countText.gameObject.SetActive(false);
        }

        if (emptyIndicator != null)
            emptyIndicator.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slot != null && !slot.IsEmpty)
        {
            OnSlotClicked?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot != null && !slot.IsEmpty)
        {
            OnSlotHoverEnter?.Invoke(slot);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnSlotHoverExit?.Invoke();
    }
}