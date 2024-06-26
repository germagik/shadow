using System;
using UnityEngine;
using Utils;

public class CharacterAnimator : MonoBehaviour
{
    #region Editable Properties
    [SerializeField] protected GameObject _lantern;
    [SerializeField] protected Transform _leftFoot;
    [SerializeField] protected Transform _rightFoot;
    [SerializeField] protected float _stepsCooldown = 0.3f;
    [SerializeField] protected float _perFootStepsCooldown = 0.5f;
    #region Surface
    [Header("Surface")]
    [SerializeField] protected LayerMask _surfaceMask;
    [SerializeField] protected float _surfaceCheckDistance = 5f;
    #endregion
    #endregion

    #region Inner Properties
    protected Animator _animator;
    public event Action<string, string> OnStep;
    protected bool _equippedLantern = false;
    protected float _stepsTimer = 0;
    protected float _leftFootStepsTimer = 0;
    protected float _rightFootStepsTimer = 0;
    #endregion

    #region Lifecycle Handlers
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void Update() 
    {
        LanternUpdate();
        StepsCooldownUpdate();
    }
    #endregion

    #region Artificial Updates
    protected virtual void LanternUpdate()
    {
        float currentWeight = _animator.GetLayerWeight((int)AnimatorLayerIndexes.Lantern);
        bool equipping = _equippedLantern && currentWeight < 1, unequipping = !_equippedLantern && currentWeight > 0;
        if (equipping || unequipping)
        {
            float next;
            if (equipping)
            {
                next = _animator.GetLayerWeight((int)AnimatorLayerIndexes.Lantern) + Time.deltaTime;
                if (next > 1)
                    next = 1;
            }
            else
            {
                next = _animator.GetLayerWeight((int)AnimatorLayerIndexes.Lantern) - Time.deltaTime;
                if (next < 0)
                    next = 0;
            }
            _animator.SetLayerWeight((int)AnimatorLayerIndexes.Lantern, next);
        }
    }
    protected virtual void StepsCooldownUpdate()
    {
        if (_stepsTimer > 0)
        {
            _stepsTimer -= Time.deltaTime;
        }
        if (_leftFootStepsTimer > 0)
        {
            _leftFootStepsTimer -= Time.deltaTime;
        }
        if (_rightFootStepsTimer > 0)
        {
            _rightFootStepsTimer -= Time.deltaTime;
        }
    }
    #endregion

    #region KeyEvents Listeners
    public virtual void TriggerOnStep(string foot)
    {
        if (_stepsTimer > 0)
        {
            return;
        }
        _stepsTimer = _stepsCooldown;
        Ray ray = new()
        {
            direction = Vector3.down
        };
        if (StepNames.IsLeft(foot) && _leftFootStepsTimer <= 0)
        {
            _leftFootStepsTimer = _perFootStepsCooldown;
            ray.origin = _leftFoot.position;
        }
        else if (StepNames.IsRight(foot) && _rightFootStepsTimer <= 0)
        {
            _rightFootStepsTimer = _perFootStepsCooldown;
            ray.origin = _rightFoot.position;
        }
        if (Physics.Raycast(ray, out RaycastHit hit, _surfaceCheckDistance, _surfaceMask))
        {
            if (hit.collider.TryGetComponent(out SurfaceSound surface))
            {
                OnStep(foot, surface.SoundReference);
            }
        }
    }
    #endregion

    #region Triggers
    public virtual void ToggleLantern()
    {
        float currentWeight = _animator.GetLayerWeight((int)AnimatorLayerIndexes.Lantern);
        bool equipping = _equippedLantern && currentWeight < 1, unequipping = !_equippedLantern && currentWeight > 0;
        if (!equipping && !unequipping)
        {
            _equippedLantern = !_equippedLantern;
            if (_equippedLantern)
                _lantern.SetActive(true);
            else
                _lantern.SetActive(false);
        }
    }
    #endregion

    #region Animator Parameters
    public virtual void SetCrouching(bool isCrouching)
    {
        _animator.SetBool(AnimatorParametersNames.IsCrouching.ToString(), isCrouching);
    }

    public virtual void SetDirection(float x, float y)
    {
        _animator.SetFloat(AnimatorParametersNames.DirectionX.ToString(), x);
        _animator.SetFloat(AnimatorParametersNames.DirectionY.ToString(), y);
    }

    public virtual void SetMotion(bool isMoving, bool isRunning = false, bool isCrouching = false)
    {
        if (isRunning && !isMoving)
        {
            isRunning = false;
        }
        _animator.SetBool(AnimatorParametersNames.IsMoving.ToString(), isMoving);
        _animator.SetBool(AnimatorParametersNames.IsRunning.ToString(), isRunning);
        _animator.SetBool(AnimatorParametersNames.IsCrouching.ToString(), isCrouching);
    }

    public virtual void SetDying()
    {
        _animator.SetTrigger("Die");
    }

    public virtual void SetAction(string action)
    {
        _animator.SetTrigger(action);
    }
    #endregion
}
