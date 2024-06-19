
using UnityEngine;

public class ExorcismWeakness : Weakness<Exorcism>
{
    protected float _maxDuration = 5f;
    protected float _duration = 0f;
    public override string Hint
    {
        get
        {
            return $"{base.Hint} ({_maxDuration - _duration})";
        }
    }

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
