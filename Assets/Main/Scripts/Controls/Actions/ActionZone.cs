using UnityEngine;
using Utils;
public abstract class ActionZone : MonoBehaviour
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    [SerializeField] protected string _blockedDescription;
    [SerializeField] protected float _distance = 3f;
    [SerializeField] protected InputAxesNames _axisName = InputAxesNames.PrimaryAction;
    public virtual string Hint
    {
        get
        {
            return $"[{_name}] {_description}";
        }
    }
    public virtual string BlockedHint
    {
        get
        {
            return $"{_blockedDescription}";
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
    public virtual bool CanBeActionatedBy(Player player)
    {
        return true;
    }
}
