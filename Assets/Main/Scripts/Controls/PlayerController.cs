using System.Collections;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] protected Transform _verticalPivot;
    [SerializeField] protected float _minVerticalAngle = -75f;
    [SerializeField] protected float _maxVerticalAngle = 75f;
    [SerializeField] protected Camera _camera;
    [SerializeField] protected Transform _lookAt;
    [SerializeField] protected float _cameraSensitivity = 2f;
    [SerializeField] protected LayerMask _actionLayers;
    [SerializeField] protected float _actionDistance = 10f;
    [SerializeField] protected float _actionCheckInterval = 0.3f;
    [SerializeField] protected HUDController _HUDController;
    protected Player _player;
    protected float _verticalRotation = 0f;
    protected ActionZone _activeAction;
    protected virtual void Awake()
    {
        _player = GetComponent<Player>();
    }

    protected virtual void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(ActionRoutine());
    }

    protected virtual void Update()
    {
        LookUpdate();
        ActionUpdate();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    protected virtual void FixedUpdate()
    {
        MoveFixedUpdate();
        ToggleLanternUpdate();
    }

    protected virtual void LookUpdate()
    {
        float inputX = Input.GetAxis(InputAxesNames.CameraX.ToString()) * _cameraSensitivity;
        float inputY = Input.GetAxis(InputAxesNames.CameraY.ToString()) * _cameraSensitivity;

        _verticalRotation -= inputY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalAngle, _maxVerticalAngle);
        _verticalPivot.localEulerAngles = Vector3.right * _verticalRotation;

        _player.transform.Rotate(Vector3.up * inputX);
        _camera.transform.LookAt(_lookAt);
    }
    protected virtual void ActionUpdate()
    {
        if (_activeAction != null)
        {
            _HUDController.SetHint(_activeAction.Hint);
            if (Input.GetAxisRaw(_activeAction.AxisName.ToString()) != 0)
            {
                _activeAction.ActionatedBy(_player);
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
                && (_camera.transform.position - actionZone.transform.position).sqrMagnitude < Mathf.Pow(actionZone.Distance, 2))
            {
                _activeAction = actionZone;
            }
            else
            {
                _HUDController.ClearHint();
                _activeAction = null;
            }
        }
        yield return new WaitForSeconds(_actionCheckInterval);
        StartCoroutine(ActionRoutine());
    }

    protected virtual void ToggleLanternUpdate()
    {
        if (Input.GetAxisRaw(InputAxesNames.Lantern.ToString()) != 0)
        {
            _player.ToggleLantern();
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
