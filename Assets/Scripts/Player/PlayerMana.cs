using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int maxMana = 100;
    private int currentMana;

    public event System.Action<int, int> OnManaChanged;

    void Start()
    {
        currentMana = maxMana;
    }

    public void Restore(float amount)
    {
        currentMana = Mathf.Min(currentMana + (int)amount, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
        Debug.Log($"현재 마나: {currentMana}/{maxMana}");
    }

    public void Use(int cost)
    {
        currentMana = Mathf.Max(currentMana - cost, 0);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }
}