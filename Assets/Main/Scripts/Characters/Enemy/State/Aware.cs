
public class Aware : EnemyState
{
    protected static Aware _instance;
    public static Aware Instance
    {
        get
        {
            return _instance ??= new();
        }
    }

    public override void HeardUpdate(Enemy enemy)
    {
        base.HeardUpdate(enemy);
        if (enemy.CheckHeard())
        {
            enemy.SetState(Alert.Instance);
        }
    }

    public override void SightUpdate(Enemy enemy)
    {
        base.SightUpdate(enemy);
        if (enemy.CheckSight())
        {
            enemy.SetState(Chasing.Instance);
        }
    }

    public override void IdleUpdate(Enemy enemy)
    {
        base.IdleUpdate(enemy);
        enemy.SetState(Unaware.Instance);
    }

}
