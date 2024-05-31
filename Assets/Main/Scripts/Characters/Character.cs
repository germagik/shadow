using System;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float _walkMaxVelocity = 3f;
    [SerializeField] protected float _walkAcceleration = 1f;
    [SerializeField] protected float _runMaxVelocity = 7f;
    [SerializeField] protected float _runAcceleration = 2f;
    [SerializeField] protected float _crouchMaxVelocity = 2f;
    [SerializeField] protected float _crouchAcceleration = 1f;
    [SerializeField] protected SoundEmitter _leftFootSoundEmitter;
    [SerializeField] protected SoundEmitter _rightFootSoundEmitter;
    [SerializeField] protected float _stepCrouchSoundFactor = 0.5f;
    [SerializeField] protected float _stepWalkSoundFactor = 1f;
    [SerializeField] protected float _stepRunSoundFactor = 2f;
    [SerializeField] protected Eyes _eyes;
    [SerializeField] protected Ears _ears;

    public virtual Eyes Eyes
    {
        get
        {
            return _eyes;
        }
    }
    protected Vector3 _direction;
    protected Rigidbody _body;
    protected Animator _animator;

    protected virtual void Awake() {
        _body = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _body.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public virtual void OnStep(string foot, string soundReference)
    {
        if (foot == StepNames.LeftFootCrouch)
        {
            _leftFootSoundEmitter.Emit(soundReference, false, false, _stepCrouchSoundFactor);
        }
        if (foot == StepNames.LeftFootWalk)
        {
            _leftFootSoundEmitter.Emit(soundReference, false, false, _stepWalkSoundFactor);
        }
        if (foot == StepNames.LeftFootRun)
        {
            _leftFootSoundEmitter.Emit(soundReference, false, false, _stepRunSoundFactor);
        }
        if (foot == StepNames.RightFootCrouch)
        {
            _rightFootSoundEmitter.Emit(soundReference, false, false, _stepCrouchSoundFactor);
        }
        if (foot == StepNames.RightFootWalk)
        {
            _rightFootSoundEmitter.Emit(soundReference, false, false, _stepWalkSoundFactor);
        }
        if (foot == StepNames.RightFootRun)
        {
            _rightFootSoundEmitter.Emit(soundReference, false, false, _stepRunSoundFactor);
        }
    }

    public virtual void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    protected virtual void Move(float maxVelocity, float acceleration, bool isRunning = false, bool isCrouching = false)
    {
        _animator.SetBool(AnimatorParametersNames.IsMoving, true);
        _animator.SetBool(AnimatorParametersNames.IsRunning, isRunning);
        _animator.SetBool(AnimatorParametersNames.IsCrouching, isCrouching);
        DoMove(_direction, maxVelocity, acceleration);
    }

    protected abstract void DoMove(Vector3 normalizedDirection, float maxVelocity, float acceleration);

    public virtual void Stay()
    {
        _animator.SetFloat(AnimatorParametersNames.DirectionY, 0);
        _animator.SetFloat(AnimatorParametersNames.DirectionX, 0);
        _animator.SetBool(AnimatorParametersNames.IsMoving, false);
        _animator.SetBool(AnimatorParametersNames.IsRunning, false);
        _animator.SetBool(AnimatorParametersNames.IsCrouching, false);
        DoStay();
    }

    protected abstract void DoStay();

    public virtual void Walk() 
    {
        Move(_walkMaxVelocity, _walkAcceleration);
    }

    public virtual void Run() 
    {
        Move(_runMaxVelocity, _runAcceleration, true);
    }

    public virtual void WalkCrouch() 
    {
        Move(_crouchMaxVelocity, _crouchAcceleration, false, true);
    }

    public virtual void Crouch()
    {
        _animator.SetBool(AnimatorParametersNames.IsCrouching, true);
    }

    public virtual void Stand()
    {
        _animator.SetBool(AnimatorParametersNames.IsCrouching, false);
    }

    public abstract void OnHear(PerceptionMark mark);
    public abstract void OnSight(PerceptionMark mark);
}
