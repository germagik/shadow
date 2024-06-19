
public class ConfineWeakness : Weakness<Confine>
{
    public override bool CanBeActionatedBy(Player player)
    {
        return base.CanBeActionatedBy(player) && player.IsCrouching;
    }

    public override void ActionatedBy(Player player)
    {
        _enemy.SetState(Bounded.Instance);
    }
}
