
public class Chasing : EnemyState
{
    public static Chasing Instance
    {
        get
        {
            return Instance<Chasing>();
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
        enemy.SetState(Unaware.Instance);
    }
}
