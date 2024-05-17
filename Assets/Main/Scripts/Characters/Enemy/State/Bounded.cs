

public class Bounded : EnemyState
{
    protected static Bounded _instance;
    public static Bounded Instance
    {
        get
        {
            return _instance ??= new();
        }
    }
}
