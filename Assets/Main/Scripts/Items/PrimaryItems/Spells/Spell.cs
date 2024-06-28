using System;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    protected float _saltCost = 1f;
    public virtual bool CanBeCastedBy(Player player)
    {
        return true;
    }
    
    public abstract void CastedBy(Player player, Action<string> Callback);
}
