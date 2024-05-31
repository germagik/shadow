using UnityEngine;

public class Bound : ActionZone
{
    [SerializeField] protected Enemy _enemy;

    public override void ActionatedBy(Player player)
    {
        
        _enemy.SetState(Bounded.Instance);
    }
}
