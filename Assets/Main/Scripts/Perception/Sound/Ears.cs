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
                if (mark.gameObject.activeSelf) {
                    return true;
                }
            }
            return false;
        }
    }

    public void OnHear(Sound sound, SoundEmitter emitter)
    {
        SetPerceptionMarkFrom(emitter.Producer, sound.transform);
        // int producerID = emitter.Producer.GetInstanceID();
        // if (producerID == _character.gameObject.GetInstanceID() || !_producerLayerMask.Includes(emitter.Producer.layer))
        // {
        //     return;
        // }
        // _marks.TryGetValue(producerID, out PerceptionMark mark);
        // if (mark != null)
        // {
        //     if (!mark.gameObject.activeSelf)
        //     {
        //         _marks.Remove(producerID);
        //         Destroy(mark.gameObject);
        //     }
        //     else
        //     {
        //         mark.transform.position = sound.transform.position;
        //         mark.ResetTime();
        //         _character.OnHear(mark);
        //     }
        // }
        // else
        // {
        //     mark = Instantiate(_heardMarkPrefab, sound.transform.position , _heardMarkPrefab.transform.rotation);
        //     _marks.Add(producerID, mark);
        //     _character.OnHear(mark);
        // }
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
