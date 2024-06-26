using UnityEngine;
using Utils;

// TP2: Facundo Campos
public class SurfaceSound : MonoBehaviour
{
    [SerializeField] protected string _soundReference = SurfaceSoundNames.Default;
    public string SoundReference
    { 
        get 
        { 
            return _soundReference;
        } 
       
    } 
}
