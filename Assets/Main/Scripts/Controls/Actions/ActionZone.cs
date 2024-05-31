using UnityEngine;
using Utils;
public abstract class ActionZone : MonoBehaviour
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    [SerializeField] protected float _distance = 3f;
    [SerializeField] protected InputAxesNames _axisName = InputAxesNames.PrimaryAction;
    public virtual string Hint
    {
        get
        {
            return $"[{_name}] {_description}";
        }
    }
    public InputAxesNames AxisName
    {
        get
        {
            return _axisName;
        }
    }

    public float Distance
    {
        get
        {
            return _distance;
        }
    }
    public abstract void ActionatedBy(Player player);
}
