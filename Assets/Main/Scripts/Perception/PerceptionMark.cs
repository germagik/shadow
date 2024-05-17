using UnityEngine;

public abstract class PerceptionMark : MonoBehaviour
{
    [SerializeField] protected float _duration = 10f;
    protected float _time = 0f;
    protected bool _keep = false;

    protected virtual void Update()
    {
        if (_time < _duration)
        {
            _time += Time.deltaTime;
        }
        else
        {
            if (!_keep)
                gameObject.SetActive(false);
        }
    }

    public virtual void ResetTime()
    {
        _time = 0f;
    }

    public virtual void Keep()
    {
        _keep = true;
    }
}
