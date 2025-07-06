using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement_PA : NetworkBehaviour, ISpawned
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private NetworkCharacterController _networkController;
    [SerializeField] private InputActionReference _moveInput;
    //[SerializeField] private InputActionReference _sprintInput;
    //[SerializeField] private InputActionReference _jumpInput;
    [FormerlySerializedAs("_speed")]
    [SerializeField] private float _walkSpeed;
    //[SerializeField] private float _sprintSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Animator _anim;

    [SerializeField] private InputActionReference _lookInput;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private PlayerSpawner _playerSpawner;

    // private bool _isJump;
    private float _rotateX;

    [Networked, OnChangedRender(nameof(OnVelocityChanged))]
    private Vector3 Velocity { get; set;}

    [Networked, OnChangedRender(nameof(TwoPlayerJoined))]
    private bool CanMove { get; set; }

    //public float currentSpeed;

    private void Start()
    {
        _playerSpawner = FindObjectOfType<PlayerSpawner>();
    }

    private void OnVelocityChanged()
    {
        _anim.SetFloat("Speed", Velocity.magnitude);
    }

    private void TwoPlayerJoined()
    {
        if (PlayerSpawner.playerCount == 2)
        {
            CanMove = true;
        }
    }

    private void OnePlayerJoined()
    {
        if (PlayerSpawner.playerCount == 1)
        {
            _playerSpawner.waitingText.gameObject.SetActive(true);
        }
    }

    public override void Spawned()
    {
        base.Spawned();
        if (!HasStateAuthority) { return; }

        _characterController.enabled = true;
    }

    private void Update()
    {
        if (!HasStateAuthority) { return; }

        TwoPlayerJoined();

        if (!CanMove)
        {
            Velocity = Vector3.zero;
            return;
        }

        Velocity = GetMoveDirection();

        Velocity *= _walkSpeed;

        //currentSpeed = Velocity.magnitude;
        //Debug.Log($"[PlayerMovement_PA] Current Speed: {currentSpeed}");

        // if (_jumpInput.action.triggered)
        // {
        //     _isJump = true;
        // }

        var lookValues = _lookInput.action.ReadValue<Vector2>();
        if (Mathf.Abs(lookValues.x) > 0.01f)
        {
            _rotateX += lookValues.x * _rotateSpeed * Runner.DeltaTime;
        }

        if (Velocity.magnitude > 0.1f)
        {
            Vector3 lookDir = new Vector3(Velocity.x, 0, Velocity.z);
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotateSpeed * Time.deltaTime);
            }
        }

        OnePlayerJoined();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        TwoPlayerJoined();
        if (!HasStateAuthority) { return; }
        NetworkUpdateMovement();
        NetworkUpdateRotation();
    }

    private void NetworkUpdateMovement()
    {
        _networkController.Move(Velocity);
        // if (_isJump)
        // {
        //     _networkController.Jump();
        //     _isJump = false;
        // }
    }

    private Vector3 GetMoveDirection()
    {
        var inputValues = _moveInput.action.ReadValue<Vector2>();
        if (_cameraTransform == null) return Vector3.zero;
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        Vector3 move = camForward * inputValues.y + camRight * inputValues.x;
        return move.normalized;
    }

    private void NetworkUpdateRotation()
    {
        if (Mathf.Abs(_rotateX) > 0.01f)
        {
            transform.Rotate(0, _rotateX, 0);
        }
        _rotateX = 0;
    }
}
