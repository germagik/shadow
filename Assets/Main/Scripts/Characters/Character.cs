using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody))]
public abstract class Character : MonoBehaviour
{
    #if UNITY_EDITOR
    public float velocity = 0;
    public Vector3 _direction = new();
    #endif
    [SerializeField] protected float _startingWalkVelocity = 0.5f;
    [SerializeField] protected float _walkMaxVelocity = 3f;
    [SerializeField] protected float _walkAcceleration = 1f;
    [SerializeField] protected float _startingRunVelocity = 2f;
    [SerializeField] protected float _runMaxVelocity = 7f;
    [SerializeField] protected float _runAcceleration = 2f;
    [SerializeField] protected float _startingCrouchVelocity = 0.5f;
    [SerializeField] protected float _crouchMaxVelocity = 2f;
    [SerializeField] protected float _crouchAcceleration = 1f;
    [SerializeField] protected float _reaction = 0.1f;
    [SerializeField] protected SoundEmitter _leftFootSoundEmitter;
    [SerializeField] protected SoundEmitter _rightFootSoundEmitter;
    protected Rigidbody _body;
    protected Animator _animator;
    protected virtual void Awake() {
        _body = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _body.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public virtual void OnStep(string foot, string soundReference)
    { 
        if (foot == StepNames.LeftFoot)
        {
            _leftFootSoundEmitter.Emit(soundReference, true, false, 1f);
        }
        else if (foot == StepNames.RightFoot)
        {
            _rightFootSoundEmitter.Emit(soundReference, true, false, 1f);
        }
    }
    protected virtual void MoveFixedUpdate(Vector3 direction, float maxVelocity, float startingVelocity, float acceleration, bool isRunning = false, bool isCrouching = false)
    {
        #if UNITY_EDITOR
        velocity = _body.velocity.magnitude;
        _direction = direction;
        #endif
        if (direction.magnitude > _reaction)
        {
            Vector3 normalizedDirection = direction.normalized;
            Vector3 relativeDirection = ((transform.forward * normalizedDirection.z) + transform.right * normalizedDirection.x).normalized;
            _animator.SetFloat(AnimatorParametersNames.DirectionY, direction.z);
            _animator.SetFloat(AnimatorParametersNames.DirectionX, direction.x);
            _animator.SetBool(AnimatorParametersNames.IsMoving, true);
            _animator.SetBool(AnimatorParametersNames.IsRunning, isRunning);
            _animator.SetBool(AnimatorParametersNames.IsCrouching, isCrouching);
            if (_body.velocity.magnitude < startingVelocity)
                _body.velocity = relativeDirection * startingVelocity;
            if (_body.velocity.magnitude < maxVelocity)
                _body.velocity += relativeDirection * acceleration;
            else
                _body.velocity = relativeDirection * maxVelocity;
        }
        else
        {
            _body.velocity = Vector3.zero;
            _animator.SetFloat(AnimatorParametersNames.DirectionY, 0);
            _animator.SetFloat(AnimatorParametersNames.DirectionX, 0);
            _animator.SetBool(AnimatorParametersNames.IsMoving, false);
            _animator.SetBool(AnimatorParametersNames.IsRunning, false);
            _animator.SetBool(AnimatorParametersNames.IsCrouching, false);
        }
    }

    public virtual void WalkFixedUpdate(float leftRight, float forwardBackward) 
    {
        MoveFixedUpdate(new Vector3(leftRight, 0, forwardBackward), _walkMaxVelocity, _startingWalkVelocity, _walkAcceleration);
    }

    public virtual void RunFixedUpdate(float leftRight, float forwardBackward) 
    {
        MoveFixedUpdate(new Vector3(leftRight, 0, forwardBackward), _runMaxVelocity, _startingRunVelocity, _runAcceleration, true);
    }

    public virtual void MoveCrouchFixedUpdate(float leftRight, float forwardBackward) 
    {
        MoveFixedUpdate(new Vector3(leftRight, 0, forwardBackward), _crouchMaxVelocity, _startingCrouchVelocity, _crouchAcceleration, false, true);
    }

    public virtual void CrouchFixedUpdate()
    {
        _animator.SetBool(AnimatorParametersNames.IsCrouching, true);
    }

    public virtual void StandFixedUpdate()
    {
        _animator.SetBool(AnimatorParametersNames.IsCrouching, false);
    }


}
