using UnityEngine;
using Utils;

[RequireComponent(typeof(Character))]
public class FirstPersonControl : MonoBehaviour
{
    #if UNITY_EDITOR
    private bool _enabled = true;
    #endif
    [SerializeField] protected Transform _verticalPivot;
    [SerializeField] protected float _minVerticalAngle = -75f;
    [SerializeField] protected float _maxVerticalAngle = 75f;
    [SerializeField] protected Camera _camera;
    [SerializeField] protected Transform _lookAt;
    [SerializeField] protected float _lookSensitivity = 2f;
    private Character _player;
    private float _verticalRotation = 0f;
    protected virtual void Awake() {
        _player = GetComponent<Character>();
    }

    protected virtual void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected virtual void Update()
    {
        #if UNITY_EDITOR
        if (_enabled)
        {
        #endif
        LookUpdate();
        #if UNITY_EDITOR
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            _enabled = !_enabled;
        }
        #endif
    }

    protected virtual void FixedUpdate()
    {
        #if UNITY_EDITOR
        if (_enabled)
        {
        #endif
        MoveFixedUpdate();
        ToggleLanternUpdate();
        #if UNITY_EDITOR
        }
        #endif
    }

    protected virtual void LookUpdate()
    {
        float inputX = Input.GetAxis(InputAxesNames.CameraX) * _lookSensitivity;
        float inputY = Input.GetAxis(InputAxesNames.CameraY) * _lookSensitivity;

        _verticalRotation -= inputY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalAngle, _maxVerticalAngle);
        _verticalPivot.localEulerAngles = Vector3.right * _verticalRotation;

        _player.transform.Rotate(Vector3.up * inputX);
        _camera.transform.LookAt(_lookAt);
    }

    protected virtual void ToggleLanternUpdate()
    {
        if (Input.GetAxisRaw(InputAxesNames.Lantern) != 0)
        {
            _player.ToggleLantern();
        }
    }

    protected virtual void MoveFixedUpdate()
    {
        float inputX = Input.GetAxis(InputAxesNames.Horizontal);
        float inputY = Input.GetAxis(InputAxesNames.Vertical);
        if (inputX != 0 || inputY != 0)
        {
            if (Input.GetAxisRaw(InputAxesNames.Run) != 0)
                _player.RunFixedUpdate(inputX, inputY);
            else if (Input.GetAxisRaw(InputAxesNames.Crouch) != 0)
                _player.MoveCrouchFixedUpdate(inputX, inputY);
            else
                _player.WalkFixedUpdate(inputX, inputY);
        }

        if (Input.GetAxisRaw(InputAxesNames.Crouch) != 0)
            _player.CrouchFixedUpdate();
        else
            _player.StandFixedUpdate();
    }
}
