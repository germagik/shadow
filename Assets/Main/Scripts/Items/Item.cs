using System;
using UnityEngine;

[Serializable]
public abstract class Item : MonoBehaviour
{
    public virtual void PickedBy(Player player)
    {
        player.AddItem(this);
        gameObject.SetActive(false);
    }
}
