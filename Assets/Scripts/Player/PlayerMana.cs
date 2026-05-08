using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int maxMana = 200;
    private int currentMana;

    public event System.Action<int, int> OnManaChanged;

    public int MaxMana => maxMana;
    public int CurrentMana => currentMana;

    void Start()
    {
        currentMana = 10;
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    public void Restore(float amount)
    {
        currentMana = Mathf.Min(currentMana + (int)amount, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
        Debug.Log($"마나 회복! 현재: {currentMana}/{maxMana}");
    }

    public void UseMana(int cost)
    {
        currentMana = Mathf.Max(currentMana - cost, 0);
        OnManaChanged?.Invoke(currentMana, maxMana);
        Debug.Log($"마나 사용! 현재: {currentMana}/{maxMana}");
    }
}