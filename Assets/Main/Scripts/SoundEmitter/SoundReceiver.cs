using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReceiver : MonoBehaviour
{
    [SerializeField] protected Character _character;
    protected List<HeardMark> _marks = new();
    [SerializeField] protected HeardMark _heardMarkPrefab;
    public void OnHear (Sound sound, SoundEmitter emitter)
    {
        HeardMark mark = GetMark(emitter.Producer.GetInstanceID());
        if (mark != null)
        {
            _marks.Remove(mark);
            Destroy(mark.gameObject);
        }
        mark = Instantiate(_heardMarkPrefab, sound.transform.position , _heardMarkPrefab.transform.rotation);
        mark.Initilize(emitter.Producer.GetInstanceID());
        _marks.Add(mark);
    }
    protected HeardMark GetMark (int producerID)
    {
        foreach (HeardMark mark in _marks)
        {
            if (mark.ProducerID == producerID)
            {  
                return mark; 
            }
        }
        return null;
    }
}
