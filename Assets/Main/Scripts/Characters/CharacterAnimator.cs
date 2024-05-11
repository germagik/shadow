using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] protected Character _character;
    [SerializeField] protected Transform _leftFoot;
    [SerializeField] protected Transform _rightFoot;
    [SerializeField] protected LayerMask _layerMask;
    public void OnStep (string foot)
    {
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        if (foot == StepNames.LeftFoot)
        {
            ray.origin = _leftFoot.position;
        }
        else if (foot == StepNames.RightFoot)
        {
            ray.origin = _rightFoot.position;
        }
        RaycastHit hit; 
        if (Physics.Raycast(ray, out hit,5f,_layerMask))
        {
            Surface surface;
            if (hit.collider.TryGetComponent<Surface>(out surface))
            {
               _character.OnStep(surface.SoundReference);
            }
        }
    }
}
