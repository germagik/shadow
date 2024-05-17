
public class Weakened : EnemyState
{
    protected static Weakened _instance;
    public static Weakened Instance
    {
        get
        {
            return _instance ??= new();
        }
    }
}
