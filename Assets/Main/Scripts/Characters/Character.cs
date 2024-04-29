using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [SerializeField] protected float _startingWalkVelocity = 2f;
    [SerializeField] protected float _walkVelocity = 5f;
    [SerializeField] protected float _walkAcceleration = 1f;
    [SerializeField] protected float _reaction = 0.1f;
    protected Rigidbody _body;
    protected Animator _animator;
    protected virtual void Awake() {
        _body = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _body.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    public virtual void WalkFixedUpdate(float leftRight, float forwardBackward) 
    {
        Vector3 direction = new(leftRight, 0, forwardBackward);
        if (direction.magnitude >= _reaction)
        {
            Vector3 normalizedDirection = direction.normalized;
            _animator.SetFloat(AnimatorParametersNames.DirectionY, forwardBackward);
            _animator.SetFloat(AnimatorParametersNames.DirectionX, leftRight);
            _animator.SetBool(AnimatorParametersNames.IsWalking, true);
            if (_body.velocity.magnitude < _startingWalkVelocity)
                _body.AddRelativeForce(normalizedDirection * _startingWalkVelocity, ForceMode.VelocityChange);
            else if (_body.velocity.magnitude < _walkVelocity)
                _body.AddRelativeForce(normalizedDirection * _walkAcceleration, ForceMode.VelocityChange);
        }
        else
        {
            _animator.SetBool(AnimatorParametersNames.IsWalking, false);
            _body.velocity = Vector3.zero;
        }
    }
}
