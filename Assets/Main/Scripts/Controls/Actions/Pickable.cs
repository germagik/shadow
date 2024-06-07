using UnityEngine;

public class Pickable : ActionZone
{
    [SerializeField] protected Item _item;

    public override void ActionatedBy(Player player)
    {
        _item.PickedBy(player);
        Destroy(gameObject);
    }

}
