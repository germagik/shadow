using UnityEngine;

[CreateAssetMenu(menuName = "Items/SpellPage")]
public class SpellPage : PrimaryItem
{
    [SerializeField] protected Spell _spell;
    public virtual Spell Spell
    {
        get
        {
            return _spell;
        }
    }

    public override void PickedBy(Player player)
    {
        player.AddPage(this);
    }
}
