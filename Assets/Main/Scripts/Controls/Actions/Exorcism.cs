
using UnityEngine;

public class Exorcism : ActionPoint
{
    [SerializeField] protected Enemy _enemy;

    protected float _maxDuration = 5f;
    protected float _duration = 0f;
    public override void ActionatedBy(Player player)
    {
        if (_duration < _maxDuration)
        {
            _duration += Time.deltaTime;
        }
        else
        {
            _enemy.gameObject.SetActive(false);
        }
    }
}
