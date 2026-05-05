using UnityEngine;

[CreateAssetMenu(fileName = "Mana Effect", menuName = "Inventory/Components/Effects/Mana")]
public class ManaEffect : ItemEffect
{
    [Header("마나 회복량")]
    public float manaAmount = 30f;

    public override void Apply(GameObject target)
    {
        PlayerMana mana = target.GetComponent<PlayerMana>();
        if (mana != null)
        {
            mana.Restore(manaAmount);
            Debug.Log($"마나 {manaAmount} 회복!");
        }
    }

    public override string GetComponentName() => "마나 회복";

    public override string GetDescription()
    {
        return $"마나 {manaAmount} 회복";
    }
}
