using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Ears : MonoBehaviour
{
    [SerializeField] protected Character _character;
    [SerializeField] protected HeardMark _heardMarkPrefab;
    [SerializeField] protected LayerMask _producerLayerMask;
    protected Dictionary<int,HeardMark> _marks = new();
    public HeardMark Closer
    {
        get
        {
            HeardMark found = null;
            var marks = _marks.Values;
            foreach (HeardMark mark in marks)
            {
                if (!mark.gameObject.activeSelf)
                {
                    continue;
                }
                found ??= mark;
                float distance = Vector3.Distance(found.transform.position, _character.transform.position);
                float currentDistance = Vector3.Distance(mark.transform.position, _character.transform.position);
                if (currentDistance < distance)
                {
                    found = mark;
                }
            }
            return found;
        }
    }
    public void OnHear(Sound sound, SoundEmitter emitter)
    {
        int producerID = emitter.Producer.GetInstanceID();
        if (producerID == _character.gameObject.GetInstanceID() || !_producerLayerMask.Includes(emitter.Producer.layer))
        {
            return;
        }
        _marks.TryGetValue(producerID, out HeardMark mark);
        if (mark != null)
        {
            if (!mark.gameObject.activeSelf)
            {
                _marks.Remove(producerID);
                Destroy(mark.gameObject);
            }
            else
            {
                mark.transform.position = sound.transform.position;
                mark.ResetTime();
                _character.OnHear(mark);
            }
        }
        else
        {
            mark = Instantiate(_heardMarkPrefab, sound.transform.position , _heardMarkPrefab.transform.rotation);
            _character.OnHear(mark);
            _marks.Add(producerID, mark);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var marks = _marks.Values;
        foreach (HeardMark mark in marks)
        {
            if (mark.gameObject.activeSelf) {
                Gizmos.DrawLine(transform.position, mark.transform.position);
            }
        }
    }

    public virtual bool HasHeard()
    {
        var marks = _marks.Values;
        foreach (HeardMark mark in marks)
        {
            if (mark.gameObject.activeSelf) {
                return true;
            }
        }
        return false;
    }
}
