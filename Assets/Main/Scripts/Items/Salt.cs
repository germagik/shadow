using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salt : Item
{
    
    protected float _count;
    public float Count
    {
        get
        {
            return _count;
        }
    }
    public override void PickedBy(Player player)
    {
        player.AddSalt(this);
        gameObject.SetActive(false);
    }

    public virtual void Add(Salt salt)
    {
        _count += salt.Count;
    }
}
