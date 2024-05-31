using System.Collections.Generic;

public abstract class EnemyState
{
    private static readonly List<EnemyState> _instances = new();
    protected static T Instance<T>() where T : EnemyState, new()
    {
        var instance = _instances.Find(instance => instance is T);
        if (instance == null)
        {
            instance = new T();
            _instances.Add(instance);
        }
        return (T)instance;
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

    public virtual void OnIn(Enemy enemy)
    {

    }

    public virtual void OnOut(Enemy enemy)
    {
        
    }
}
