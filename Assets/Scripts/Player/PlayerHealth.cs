using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 200;
    private int currentHealth;

    public event System.Action<int, int> OnHealthChanged;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = 10;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + (int)amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"체력 회복! 현재: {currentHealth}/{maxHealth}");
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"피해! 현재: {currentHealth}/{maxHealth}");
    }
}