using UnityEngine;

public abstract class ItemComponent : ScriptableObject
{
    public abstract string GetComponentName();
    public abstract string GetDescription();
}