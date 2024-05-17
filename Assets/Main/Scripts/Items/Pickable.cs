using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] protected Item _item;

    public virtual void PickedBy(Player player)
    {
        _item.PickedBy(player);
        Destroy(gameObject);
    }
}
