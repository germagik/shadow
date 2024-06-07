using UnityEngine;

public class Bound : ActionZone
{
    [SerializeField] protected Enemy _enemy;

    public override bool CanBeActionatedBy(Player player)
    {
        return player.IsCrouching;
    }
    public override void ActionatedBy(Player player)
    {
        _enemy.SetState(Bounded.Instance);
    }
}
