
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Salt")]
public class Salt : Item
{
    
    [SerializeField] protected float _count;
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
    }

    public virtual void Add(Salt salt)
    {
        _count += salt.Count;
    }
}
