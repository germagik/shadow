using System;
using UnityEngine;

public class SightMark : PerceptionMark
{
    [SerializeField] protected float _followDuration = 3f;
    protected float _followTime = 0f;
    protected GameObject _target;

    protected override void Update()
    {
        if (_followTime < _followDuration)
        {
            _followTime += Time.deltaTime;
            transform.position = _target.transform.position;
        }
        else
        {
            base.Update();
        }
    }

    public override void ResetTime()
    {
        base.ResetTime();
        _followTime = 0f;
    }

    public virtual void Initialize(GameObject target)
    {
        _target = target;
    }
}
