using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("기본 스탯")]
    public int level = 1;
    public int strength = 10;
    public int dexterity = 10;
    public int intelligence = 10;
    public int vitality = 10;

    [Header("전투 스탯")]
    private int baseAttackPower = 10;
    private int bonusAttackPower = 0;
    private int baseDefense = 5;
    private int bonusDefense = 0;
    private float attackSpeed = 1f;
    private float attackRange = 2f;

    public int TotalAttackPower => baseAttackPower + bonusAttackPower;
    public int TotalDefense => baseDefense + bonusDefense;

    public void AddAttackPower(int amount)
    {
        bonusAttackPower += amount;
    }

    public void RemoveAttackPower(int amount)
    {
        bonusAttackPower -= amount;
    }

    public void AddDefense(int amount)
    {
        bonusDefense += amount;
    }

    public void RemoveDefense(int amount)
    {
        bonusDefense -= amount;
    }

    public void SetAttackSpeed(float speed)
    {
        attackSpeed = speed;
    }

    public void ResetAttackSpeed()
    {
        attackSpeed = 1f;
    }

    public void SetAttackRange(float range)
    {
        attackRange = range;
    }

    public void ResetAttackRange()
    {
        attackRange = 2f;
    }

    public void AddMaxHealth(int amount)
    {
        // 구현
    }

    public void RemoveMaxHealth(int amount)
    {
        // 구현
    }

    public void ApplyBuff(BuffEffect.BuffType buffType, int amount, float duration)
    {
        StartCoroutine(BuffCoroutine(buffType, amount, duration));
    }

    IEnumerator BuffCoroutine(BuffEffect.BuffType buffType, int amount, float duration)
    {
        switch (buffType)
        {
            case BuffEffect.BuffType.Strength: strength += amount; break;
            case BuffEffect.BuffType.Dexterity: dexterity += amount; break;
            case BuffEffect.BuffType.Intelligence: intelligence += amount; break;
            case BuffEffect.BuffType.Vitality: vitality += amount; break;
        }
        Debug.Log($"{buffType} +{amount} 버프 시작 ({duration}초)");

        yield return new WaitForSeconds(duration);

        switch (buffType)
        {
            case BuffEffect.BuffType.Strength: strength -= amount; break;
            case BuffEffect.BuffType.Dexterity: dexterity -= amount; break;
            case BuffEffect.BuffType.Intelligence: intelligence -= amount; break;
            case BuffEffect.BuffType.Vitality: vitality -= amount; break;
        }
        Debug.Log($"{buffType} +{amount} 버프 종료");
    }
}