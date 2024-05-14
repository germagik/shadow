using System;

public class Matches : Item
{
    protected int _count;
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
        gameObject.SetActive(false);
    }

    public virtual void Add(Matches matches)
    {
        _count += matches.Count;
    }
}
