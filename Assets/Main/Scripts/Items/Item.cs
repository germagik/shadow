using UnityEngine;

public abstract class Item : ScriptableObject
{
    public virtual void PickedBy(Player player)
    {
        player.AddItem(this);
    }
}
