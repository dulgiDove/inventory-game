using UnityEngine;

public abstract class ItemEffect : ItemComponent
{
    public abstract void Apply(GameObject target);
}