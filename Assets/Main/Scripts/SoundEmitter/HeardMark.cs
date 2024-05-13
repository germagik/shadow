using UnityEngine;

public class HeardMark : MonoBehaviour
{
    [SerializeField] protected float _duration = 10f;
    [SerializeField] protected float _time = 0f;

    protected virtual void Update()
    {
        if (_time < _duration)
        {
            _time += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void ResetTime()
    {
        _time = 0f;
    }
}
