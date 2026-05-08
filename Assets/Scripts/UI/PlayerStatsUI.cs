using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;

    [Header("플레이어")]
    [SerializeField] private Player player;

    private PlayerHealth playerHealth;
    private PlayerMana playerMana;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        if (player != null)
        {
            playerHealth = player.Health;
            playerMana = player.Mana;

            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged += UpdateHealthUI;
                UpdateHealthUI(playerHealth.CurrentHealth, playerHealth.MaxHealth);
            }

            if (playerMana != null)
            {
                playerMana.OnManaChanged += UpdateManaUI;
                UpdateManaUI(playerMana.CurrentMana, playerMana.MaxMana);
            }
        }
        else
        {
            Debug.LogError("Player를 찾을 수 없습니다!");
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }

        if (playerMana != null)
        {
            playerMana.OnManaChanged -= UpdateManaUI;
        }
    }

    void UpdateHealthUI(int current, int max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }

        if (healthText != null)
        {
            healthText.text = $"{current} / {max}";
        }
    }

    void UpdateManaUI(int current, int max)
    {
        if (manaSlider != null)
        {
            manaSlider.maxValue = max;
            manaSlider.value = current;
        }

        if (manaText != null)
        {
            manaText.text = $"{current} / {max}";
        }
    }
}