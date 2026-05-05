using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Inventory/Components/Effects/Heal")]
public class HealEffect : ItemEffect
{
    [Header("체력 회복량")]
    public float healAmount = 50f;

    public override void Apply(GameObject target)
    {
        PlayerHealth health = target.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.Heal(healAmount);
            Debug.Log($"체력 {healAmount} 회복!");
        }
    }

    public override string GetComponentName() => "체력 회복";

    public override string GetDescription()
    {
        return $"체력 {healAmount} 회복";
    }
}