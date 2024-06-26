using UnityEngine;

// TP2: Carlos Ayasca
// + Todas las subclases
public abstract class Item : ScriptableObject
{
    public virtual void PickedBy(Player player)
    {
        player.AddItem(this);
    }
}
