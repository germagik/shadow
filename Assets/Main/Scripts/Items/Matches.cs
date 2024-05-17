

using UnityEngine;

[CreateAssetMenu(menuName = "Items/Matches")]
public class Matches : Item
{
    [SerializeField] protected int _count;
    public int Count
    {
        get
        {
            return _count;
        }
    }
    public override void PickedBy(Player player)
    {
        player.AddMatches(this);
    }

    public virtual void Add(Matches matches)
    {
        _count += matches.Count;
    }
}
