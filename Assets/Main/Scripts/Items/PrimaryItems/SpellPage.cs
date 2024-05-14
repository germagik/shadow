public class SpellPage : PrimaryItem
{
    public override void PickedBy(Player player)
    {
        player.AddPage(this);
        gameObject.SetActive(false);
    }
}
