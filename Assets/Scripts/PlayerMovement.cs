using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : NetworkBehaviour, ISpawned
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private NetworkCharacterController _networkController;
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _sprintInput;
    [SerializeField] private InputActionReference _jumpInput;
    [FormerlySerializedAs("_speed")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Animator _anim;

    [SerializeField] private InputActionReference _lookInput;

    private bool _isJump;
    private float _rotateX;

    [Networked, OnChangedRender(nameof(OnVelocityChanged))]
    private Vector3 Velocity { get; set;}

    private void OnVelocityChanged()
    {
        _anim.SetFloat("Speed", Velocity.magnitude);
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

        Velocity = GetMoveDirection();

        if (_sprintInput.action.IsPressed())
        {
            Velocity *= _sprintSpeed;
        }
        else
        {
            Velocity *= _walkSpeed;
        }

        if (_jumpInput.action.triggered)
        {
            _isJump = true;
        }

        var lookValues = _lookInput.action.ReadValue<Vector2>();
        _rotateX += lookValues.x * _rotateSpeed * Time.deltaTime;
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (!HasStateAuthority) { return; }
        NetworkUpdateMovement();
        NetworkUpdateRotation();
    }

    private void NetworkUpdateMovement()
    {
        _networkController.Move(Velocity);
        if (_isJump)
        {
            _networkController.Jump();
            _isJump = false;
        }
    }

    private Vector3 GetMoveDirection()
    {
        var inputValues = _moveInput.action.ReadValue<Vector2>();
        var direction = transform.forward * inputValues.y + transform.right * inputValues.x;
        return direction.normalized;
    }

    private void NetworkUpdateRotation()
    {
        transform.Rotate(0, _rotateX, 0);
        _rotateX = 0;
    }
}
