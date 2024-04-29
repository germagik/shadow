using UnityEngine;

[RequireComponent(typeof(Character))]
public class FirstPersonControl : MonoBehaviour
{
    [SerializeField] protected Transform _verticalPivot;
    [SerializeField] protected float _minVerticalAngle = -60f;
    [SerializeField] protected float _maxVerticalAngle = 70f;
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
        LookUpdate();
    }

    protected virtual void FixedUpdate()
    {
        WalkFixedUpdate();
    }

    protected virtual void LookUpdate()
    {
        float inputX = Input.GetAxis("Mouse X") * _lookSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * _lookSensitivity;

        _verticalRotation -= inputY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalAngle, _maxVerticalAngle);
        _verticalPivot.localEulerAngles = Vector3.right * _verticalRotation;

        _player.transform.Rotate(Vector3.up * inputX);
        _camera.transform.LookAt(_lookAt);
    }

    protected virtual void WalkFixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        if (inputX != 0 || inputY != 0)
            _player.WalkFixedUpdate(inputX, inputY);
    }
}
