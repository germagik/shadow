using System;
using System.Collections.Generic;
using UnityEngine;
// TP2: Facundo Campos
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
    public virtual void Emit(string soundReference, bool child, bool loop, float intensityFactor)
    {
        SoundConfiguration soundConfiguration = GetConfiguration(soundReference);
        if (soundConfiguration != null)
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
            sound.Initialize(this, soundConfiguration.RandomClip(), loop, soundConfiguration.Intensity * intensityFactor);
        }
    }
    protected virtual SoundConfiguration GetConfiguration(string soundReference)
    {
        foreach (var sound in _soundConfigurations)
        {
            if (sound.Reference == soundReference)
            {
                return sound;
            }
        }
        return null;
    }
}

[Serializable]
public class SoundConfiguration
{
    [SerializeField] protected string _reference;
    public string Reference
    {
        get
        {
            return _reference;
        }
    }
    [SerializeField] protected float _intensity;
    public float Intensity
    {
        get
        {
            return _intensity;
        }
    }
    [SerializeField] protected List<AudioClip> _clips;

    public virtual AudioClip RandomClip()
    {
        return _clips[UnityEngine.Random.Range(0,_clips.Count)];
    }
}
