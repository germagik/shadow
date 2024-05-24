using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    [SerializeField] protected LayerMask _earsLayers;
    protected AudioSource _audioSource;
    protected SoundEmitter _emittedBy;
    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public virtual void Initialize(SoundEmitter emitter, AudioClip audioClip,bool loop,float intensity)
    {
        _audioSource.clip = audioClip;
        _audioSource.loop = loop;
        _audioSource.maxDistance = intensity;
        _emittedBy = emitter;
    }

    protected virtual void Start() 
    {
        _audioSource.Play();
    }

    protected virtual void Update() 
    {
        if (!_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        Collider[] closeEars = Physics.OverlapSphere(transform.position,_audioSource.maxDistance, _earsLayers);
        foreach (Collider receiver in closeEars)
        {
            if (receiver.TryGetComponent(out Ears ears))
            {
                ears.OnHear(this, _emittedBy);
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _audioSource.maxDistance);    
    }

}
