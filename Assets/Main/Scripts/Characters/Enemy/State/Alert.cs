

using UnityEngine;

public class Alert : EnemyState
{
    protected static Alert _instance;
    public static Alert Instance
    {
        get
        {
            return _instance ??= new();
        }
    }

    public override void HeardUpdate(Enemy enemy)
    {
        base.HeardUpdate(enemy);
        enemy.SearchHeard();
    }

    public override void SightUpdate(Enemy enemy)
    {
        base.SightUpdate(enemy);
        Debug.Log("Chasing");
        enemy.SetState(Chasing.Instance);
    }

    public override void IdleUpdate(Enemy enemy)
    {
        base.IdleUpdate(enemy);
        Debug.Log("Unaware");
        enemy.SetState(Unaware.Instance);
    }
}
