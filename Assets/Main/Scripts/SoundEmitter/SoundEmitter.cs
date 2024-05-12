using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    [SerializeField] protected List<SoundConfiguration> _soundConfigurations;
    [SerializeField] protected Sound _soundPrefab;
    [SerializeField] protected GameObject _producer;

    public GameObject Producer
    {
        get
        {
            return _producer;
        }
    }
    public virtual void Emit(string soundReference, bool child, bool loop, float intensity)
    {
        AudioClip soundClip = RandomClip(soundReference);
        if (soundClip != null)
        {
            Sound sound;
            if (child)
            {
                sound = Instantiate(_soundPrefab, transform);
            }
            else
            {
                sound = Instantiate(_soundPrefab,transform.position, _soundPrefab.transform.rotation);
            }
            sound.Initialize(this, soundClip,loop,intensity);
        }
    }
    protected virtual AudioClip RandomClip (string soundReference)
    {
        foreach (var sound in _soundConfigurations)
        {
            if (sound.reference == soundReference)
            {
                return sound.clips[UnityEngine.Random.Range(0,sound.clips.Count)];
            }
        }
        return null;
    }
}

[Serializable]
public class SoundConfiguration
{
    public string reference;
    public List<AudioClip> clips;
}
