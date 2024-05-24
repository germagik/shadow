using UnityEngine;

public class ChaseMark : PerceptionMark
{
    [SerializeField] protected float _chaseDuration = 3f;
    protected float _chaseTime = 0f;
    protected GameObject _producer;

    protected override void Update()
    {
        if (_chaseTime < _chaseDuration)
        {
            _chaseTime += Time.deltaTime;
            RefreshPosition(_producer);
        }
        else
        {
            base.Update();
        }
    }

    public override void ResetTime()
    {
        base.ResetTime();
        _chaseTime = 0f;
    }

    public override void RefreshPosition(GameObject producer, Transform origin = null)
    {
        if (_chaseTime < _chaseDuration)
        {
            transform.position = producer.transform.position;
        }
    }

    public override void Initialize(GameObject producer)
    {
        _producer = producer;
    }
}
