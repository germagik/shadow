using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    protected AudioSource _audioSource;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public virtual void Initialize(AudioClip audioClip,bool loop,float intensity)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
