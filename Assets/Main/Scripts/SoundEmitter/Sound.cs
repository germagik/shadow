using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    protected AudioSource _audioSource;
    [SerializeField] protected LayerMask _layerMask;
    protected SoundEmitter _emittedBy;
    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
       
    }
    public virtual void Initialize(SoundEmitter emitter, AudioClip audioClip,bool loop,float intensity)
    {
        _audioSource.clip = audioClip;
        _emittedBy = emitter;
        _audioSource.loop = loop;
        
    }

    public virtual void Update() 
    {
        if (!_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
        
    }

    public virtual void FixedUpdate()
    {
        Collider[] receivers = Physics.OverlapSphere(transform.position,_audioSource.maxDistance, _layerMask);
        foreach (Collider receiver in receivers)
        {
            SoundReceiver soundReceiver;
            if (receiver.TryGetComponent<SoundReceiver>(out soundReceiver))
            {
                soundReceiver.OnHear(this, _emittedBy);
            }
        }
    }

    public virtual void Start() 
    {
        _audioSource.Play();
    }
}
