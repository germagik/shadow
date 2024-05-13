using System.Collections.Generic;
using UnityEngine;

public class SoundReceiver : MonoBehaviour
{
    [SerializeField] protected Character _character;
    [SerializeField] protected HeardMark _heardMarkPrefab;
    protected Dictionary<int,HeardMark> _marks = new();
    public void OnHear(Sound sound, SoundEmitter emitter)
    {
        int producerID = emitter.Producer.GetInstanceID();
        if (producerID == _character.gameObject.GetInstanceID())
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
            }
        }
        else
        {
            mark = Instantiate(_heardMarkPrefab, sound.transform.position , _heardMarkPrefab.transform.rotation);
            _marks.Add(producerID, mark);
        }
    }
}
