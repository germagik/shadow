using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    [SerializeField] protected List<SoundConfiguration> _soundConfigurations;
    [SerializeField] protected Sound _soundPrefab;
    public virtual void Emit(string soundReference, bool world, bool loop, float intensity)
    {
        AudioClip soundClip = RandomClip(soundReference);
        //Debug.Log(soundReference);
        if (soundClip != null)
        {
            Sound sound = Instantiate(_soundPrefab,transform,world);
            sound.Initialize(soundClip,loop,intensity);
            //Debug.Log(soundReference);
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
