using System.Linq;
using UnityEngine;

public class Ears : Sense
{
    public virtual bool HasHeard
    {
        get
        {
            var marks = _marks.Values.ToArray();
            foreach (PerceptionMark mark in marks)
            {
                if (mark.gameObject.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void OnHear(Sound sound, SoundEmitter emitter)
    {
        SetPerceptionMarkFrom(emitter.Producer, sound.transform);
    }

    protected override void OnFirstSense(PerceptionMark mark)
    {
        _character.OnHear(mark);
    }

    protected override void OnSense(PerceptionMark mark)
    {
        _character.OnHear(mark);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var marks = _marks.Values;
        foreach (PerceptionMark mark in marks)
        {
            if (mark.gameObject.activeSelf)
            {
                Gizmos.DrawLine(transform.position, mark.transform.position);
            }
        }
    }
}
