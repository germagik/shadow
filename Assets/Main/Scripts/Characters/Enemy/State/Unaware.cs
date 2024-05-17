public class Unaware : EnemyState
{
    protected static Unaware _instance;
    public static Unaware Instance
    {
        get
        {
            return _instance ??= new();
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
}
