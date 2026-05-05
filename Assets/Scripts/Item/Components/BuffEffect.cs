using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Inventory/Components/Effects/Buff")]
public class BuffEffect : ItemEffect
{
    public enum BuffType { Strength, Dexterity, Intelligence, Vitality }

    [Header("버프 정보")]
    public BuffType buffType;
    public int buffAmount = 5;
    public float duration = 30f;

    public override void Apply(GameObject target)
    {
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.ApplyBuff(buffType, buffAmount, duration);
            Debug.Log($"{buffType} +{buffAmount} 버프 적용! ({duration}초)");
        }
    }

    public override string GetComponentName() => "버프";

    public override string GetDescription()
    {
        return $"{buffType} +{buffAmount} ({duration}초)";
    }
}