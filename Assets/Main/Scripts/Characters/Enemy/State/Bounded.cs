

using UnityEngine;

public class Bounded : EnemyState
{
    public static Bounded Instance
    {
        get
        {
            return Instance<Bounded>();
        }
    }

    public override void Update(Enemy enemy)
    {
        base.Update(enemy);
        if (!enemy.CheckBounded())
        {
            enemy.SetState(Alert.Instance);
        }
    }

    public override void OnIn(Enemy enemy)
    {
        base.OnIn(enemy);
        enemy.OnBounded();
    }

    public override void OnOut(Enemy enemy)
    {
        base.OnOut(enemy);
        enemy.OnUnbounded();
    }
}
