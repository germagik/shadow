public class Unaware : EnemyState
{
    public static Unaware Instance
    {
        get
        {
            return Instance<Unaware>();
        }
    }

    public override void HeardUpdate(Enemy enemy)
    {
        base.HeardUpdate(enemy);
        enemy.SetState(Aware.Instance);
    }

    public override void SightUpdate(Enemy enemy)
    {
        base.SightUpdate(enemy);
        enemy.SetState(Aware.Instance);
    }

    public override void IdleUpdate(Enemy enemy)
    {
        base.IdleUpdate(enemy);
        enemy.IdleUpdate();
    }

    public override void OnIn(Enemy enemy)
    {
        base.OnIn(enemy);
        enemy.ActivateWeakness();
    }

    public override void OnOut(Enemy enemy)
    {
        base.OnOut(enemy);
        enemy.DeactivateWeakness();
    }
}
