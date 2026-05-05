using UnityEngine;

public abstract class RequirementComponent : ItemComponent
{
    public abstract bool IsMet(GameObject target);
}