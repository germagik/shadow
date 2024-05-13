using UnityEngine;
using Utils;

public class CharacterKeyEvents : MonoBehaviour
{
    [SerializeField] protected Character _character;
    [SerializeField] protected Transform _leftFoot;
    [SerializeField] protected Transform _rightFoot;
    [SerializeField] protected LayerMask _surfaceMask;
    [SerializeField] protected float _surfaceCheckDistance = 5f;
    public virtual void OnStep(string foot)
    {
        Ray ray = new()
        {
            direction = Vector3.down
        };
        if (foot == StepNames.LeftFoot)
        {
            ray.origin = _leftFoot.position;
        }
        else if (foot == StepNames.RightFoot)
        {
            ray.origin = _rightFoot.position;
        }
        if (Physics.Raycast(ray, out RaycastHit hit, _surfaceCheckDistance, _surfaceMask))
        {
            if (hit.collider.TryGetComponent(out Surface surface))
            {
                _character.OnStep(foot, surface.SoundReference);
            }
        }
    }
}
