using UnityEngine;
using Utils;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    #if UNITY_EDITOR
    private bool _enabled = true;
    public float axisH = 0f;
    public float axisV = 0f;
    #endif
    [SerializeField] protected Transform _verticalPivot;
    [SerializeField] protected float _minVerticalAngle = -75f;
    [SerializeField] protected float _maxVerticalAngle = 75f;
    [SerializeField] protected Camera _camera;
    [SerializeField] protected Transform _lookAt;
    [SerializeField] protected float _cameraSensitivity = 2f;
    [SerializeField] protected LayerMask _actionLayers;
    [SerializeField] protected float _actionsDistance = 2f;
    private Player _player;
    private float _verticalRotation = 0f; 
    protected virtual void Awake() {
        _player = GetComponent<Player>();
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
        ActionUpdate();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
        float inputX = Input.GetAxis(InputAxesNames.CameraX) * _cameraSensitivity;
        float inputY = Input.GetAxis(InputAxesNames.CameraY) * _cameraSensitivity;

        _verticalRotation -= inputY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalAngle, _maxVerticalAngle);
        _verticalPivot.localEulerAngles = Vector3.right * _verticalRotation;

        _player.transform.Rotate(Vector3.up * inputX);
        _camera.transform.LookAt(_lookAt);
    }
    protected virtual void ActionUpdate()
    {
        if(Input.GetAxisRaw(InputAxesNames.Pick) != 0)
        {
            Ray actionRay = new(_camera.transform.position, _camera.transform.forward);
            if(Physics.Raycast(actionRay, out RaycastHit actionHit, _actionsDistance, _actionLayers))
            {
                if(actionHit.collider.TryGetComponent(out Pickable item))
                {
                    item.PickedBy(_player);
                }
                if(actionHit.collider.TryGetComponent(out ActionPoint action))
                {
                    action.ActionatedBy(_player);
                }
            }
        }
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
            _player.SetDirection(new Vector3(inputX, 0, inputY));
            if (Input.GetAxisRaw(InputAxesNames.Run) != 0)
                _player.Run();
            else if (Input.GetAxisRaw(InputAxesNames.Crouch) != 0)
                _player.WalkCrouch();
            else
                _player.Walk();
        }
        else
        {
            _player.Stay();
        }

        if (Input.GetAxisRaw(InputAxesNames.Crouch) != 0)
            _player.Crouch();
        else
            _player.Stand();
    }
}
