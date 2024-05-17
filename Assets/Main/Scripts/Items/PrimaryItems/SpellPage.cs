using UnityEngine;

[CreateAssetMenu(menuName = "Items/SpellPage")]
public class SpellPage : PrimaryItem
{
    public override void PickedBy(Player player)
    {
        player.AddPage(this);
    }
}
