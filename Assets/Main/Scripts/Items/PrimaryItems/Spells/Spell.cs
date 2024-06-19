using UnityEngine;

public abstract class Spell : ScriptableObject
{
    protected float _saltCost = 1f;
    public virtual bool CanBeCastedBy(Player player)
    {
        return false;
    }
    
    public abstract bool CastedBy(Player player);
}
