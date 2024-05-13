using UnityEngine;
using Utils;

public class Surface : MonoBehaviour
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
