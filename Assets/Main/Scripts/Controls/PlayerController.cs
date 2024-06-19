using System;
using System.Collections;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] protected Transform _verticalPivot;
    [SerializeField] protected Transform _spine;
    [SerializeField] protected float _minVerticalAngle = -75f;
    [SerializeField] protected float _maxVerticalAngle = 75f;
    [SerializeField] protected float _spineMinVerticalAngle = -50f;
    [SerializeField] protected float _spineMaxVerticalAngle = 50f;
    [SerializeField] protected float _spineCrouchMinVerticalAngle = -50f;
    [SerializeField] protected float _spineCrouchMaxVerticalAngle = 20f;
    [SerializeField] protected Camera _camera;
    [SerializeField] protected Transform _lookAt;
    [SerializeField] protected float _cameraSensitivity = 2f;
    [SerializeField] protected LayerMask _actionLayers;
    [SerializeField] protected float _actionDistance = 10f;
    [SerializeField] protected float _actionCheckInterval = 0.3f;
    [SerializeField] protected UIController _HUDController;
    protected Player _player;
    protected float _verticalRotation = 0f;
    protected float _spineVerticalRotation = 0f;
    protected ActionZone _aimedAction;
    protected Action StateUpdate;
    protected Action StateFixedUpdate;
    protected virtual void Awake()
    {
        _player = GetComponent<Player>();
        StateUpdate = PlayerControlUpdate;
        StateFixedUpdate = PlayerControlFixedUpdate;
    }

    protected virtual void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(ActionRoutine());
    }

    protected virtual void Update()
    {
        StateUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateFixedUpdate();
    }

    protected void PlayerControlUpdate()
    {
        if (_player.IsGrabbed || _player.IsDead)
        {
            return;
        }
        CameraUpdate();
        AimUpdate();
        CommandsUpdate();

    }
    protected virtual void PlayerControlFixedUpdate()
    {
        if (_player.IsGrabbed || _player.IsDead)
        {
            _player.Stand();
            _player.Stay();
            return;
        }
        MoveFixedUpdate();
    }

    protected virtual void GoingToTargetUpdate()
    {

    }
    protected virtual void GoingToTargetFixedUpdate()
    {
        CrouchingFixedUpdate();
        if (!(_aimedAction != null && _aimedAction.CanBeActionatedBy(_player)))
        {
            _player.Stay();
            StateUpdate = PlayerControlUpdate;
            StateFixedUpdate = PlayerControlFixedUpdate;
            return;
        }
        if ((_aimedAction.transform.position - transform.position).sqrMagnitude <= Mathf.Pow(_aimedAction.RequiredDistance, 2))
        {
            _aimedAction.ActionatedBy(_player);
            _player.Stay();
            StateUpdate = PlayerControlUpdate;
            StateFixedUpdate = PlayerControlFixedUpdate;
            return;
        }
        if (_player.IsCrouching)
        {
            _player.WalkCrouch();
        }
        else
        {
            _player.Walk();
        }
    }

    protected virtual void CameraUpdate()
    {
        float inputX = Input.GetAxis(InputAxesNames.CameraX.ToString()) * _cameraSensitivity;
        float inputY = Input.GetAxis(InputAxesNames.CameraY.ToString()) * _cameraSensitivity;

        _verticalRotation -= inputY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalAngle, _maxVerticalAngle);
        _verticalPivot.localEulerAngles = Vector3.right * _verticalRotation;

        _spineVerticalRotation = _verticalRotation;
        if (_player.IsCrouching)
        {
            _spineVerticalRotation = Mathf.Clamp(_spineVerticalRotation, _spineCrouchMinVerticalAngle, _spineCrouchMaxVerticalAngle);
            _spine.localEulerAngles = Vector3.right * _spineVerticalRotation;
        }
        else
        {
            _spineVerticalRotation = Mathf.Clamp(_spineVerticalRotation, _spineMinVerticalAngle, _spineMaxVerticalAngle);
            _spine.localEulerAngles = Vector3.right * _spineVerticalRotation;
        }

        _player.transform.Rotate(Vector3.up * inputX);
        _camera.transform.LookAt(_lookAt);
    }

    protected virtual void AimUpdate()
    {
        if (_aimedAction != null)
        {
            if (_aimedAction.CanBeActionatedBy(_player))
            {
                _HUDController.SetHint(_aimedAction.Hint);
                if (Input.GetAxisRaw(_aimedAction.AxisName.ToString()) != 0)
                {
                    StateUpdate = GoingToTargetUpdate;
                    StateFixedUpdate = GoingToTargetFixedUpdate;
                }
            }
            else
            {
                _HUDController.SetHint(_aimedAction.BlockedHint);
            }
        }
    }

    protected virtual IEnumerator ActionRoutine()
    {
        if (_player.Eyes.HasActions)
        {
            Ray actionRay = new(_camera.transform.position, _camera.transform.forward);
            if (Physics.Raycast(actionRay, out RaycastHit actionHit, _actionDistance, _actionLayers)
                && actionHit.collider.TryGetComponent(out ActionZone actionZone)
                && (_camera.transform.position - actionZone.transform.position).sqrMagnitude < Mathf.Pow(actionZone.SightDistance, 2))
            {
                _aimedAction = actionZone;
            }
            else
            {
                _HUDController.ClearHint();
                _aimedAction = null;
            }
        }
        yield return new WaitForSeconds(_actionCheckInterval);
        StartCoroutine(ActionRoutine());
    }

    protected virtual void CommandsUpdate()
    {
        if (Input.GetAxisRaw(InputAxesNames.Lantern.ToString()) != 0)
        {
            _player.ToggleLantern();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    protected virtual void MoveFixedUpdate()
    {
        float inputX = Input.GetAxis(InputAxesNames.Horizontal.ToString());
        float inputY = Input.GetAxis(InputAxesNames.Vertical.ToString());
        if (inputX != 0 || inputY != 0)
        {
            _player.SetDirection(new Vector3(inputX, 0, inputY));
            if (Input.GetAxisRaw(InputAxesNames.Run.ToString()) != 0)
            {
                _player.Run();
            }
            else if (Input.GetAxisRaw(InputAxesNames.Crouch.ToString()) != 0)
            {
                _player.WalkCrouch();
            }
            else
            {
                _player.Walk();
            }
        }
        else
        {
            _player.Stay();
        }

        CrouchingFixedUpdate();
    }

    protected virtual void CrouchingFixedUpdate()
    {
        if (Input.GetAxisRaw(InputAxesNames.Crouch.ToString()) != 0)
        {
            _player.Crouch();
        }
        else
        {
            _player.Stand();
        }
    }
}
