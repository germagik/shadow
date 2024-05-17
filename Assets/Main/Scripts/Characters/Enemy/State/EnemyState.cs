
using System;

public abstract class EnemyState
{
    public static EnemyState Initial
    {
        get
        {
            return Unaware.Instance;
        }
    }

    protected EnemyState()
    {

    }

    public virtual void HeardFixedUpdate(Enemy enemy)
    {

    }
    public virtual void HeardUpdate(Enemy enemy)
    {

    }
    public virtual void IdleFixedUpdate(Enemy enemy)
    {

    }
    public virtual void IdleUpdate(Enemy enemy)
    {

    }
    public virtual void SightFixedUpdate(Enemy enemy)
    {

    }
    public virtual void SightUpdate(Enemy enemy)
    {

    }

    public virtual void Update(Enemy enemy)
    {
        
    }
}
