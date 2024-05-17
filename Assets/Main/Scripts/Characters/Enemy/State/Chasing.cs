
using UnityEngine;

public class Chasing : EnemyState
{
    protected static Chasing _instance;
    public static Chasing Instance
    {
        get
        {
            return _instance ??= new();
        }
    }

    public override void Update(Enemy enemy)
    {
        base.Update(enemy);
        enemy.Chase();
    }

    // public override void SightUpdate(Enemy enemy)
    // {
    //     base.SightUpdate(enemy);
    //     enemy.Chase();
    // }

    public override void IdleUpdate(Enemy enemy)
    {
        base.IdleUpdate(enemy);
        // Debug.Log("Unaware");
        // enemy.SetState(Unaware.Instance);
    }
}
