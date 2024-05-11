using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Surface : MonoBehaviour
{
    [SerializeField] protected string _soundReference = SurfaceSoundNames.Defoult;
    public string SoundReference
    { 
        get 
        { 
            return _soundReference;
        } 
       
    } 
}
