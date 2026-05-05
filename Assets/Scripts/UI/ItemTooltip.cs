using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform rectTransform;

    void Update()
    {
        if (gameObject.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            rectTransform.position = mousePos + new Vector3(20, -20, 0);

            ClampToScreen();
        }
    }

    public void Show(ItemData item)
    {
        gameObject.SetActive(true);

        nameText.text = item.itemName;
        descriptionText.text = item.GetFullDescription();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void ClampToScreen()
    {
        Vector3 pos = rectTransform.position;

        float halfWidth = rectTransform.rect.width / 2f;
        float halfHeight = rectTransform.rect.height / 2f;

        pos.x = Mathf.Clamp(pos.x, halfWidth, Screen.width - halfWidth);
        pos.y = Mathf.Clamp(pos.y, halfHeight, Screen.height - halfHeight);

        rectTransform.position = pos;
    }
}