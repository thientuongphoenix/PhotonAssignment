using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_PA : MonoBehaviour
{
    [SerializeField] protected CharacterController _characterController;
    [SerializeField] protected InputActionReference _moveInput;
    [SerializeField] protected float _speed = 2f;

    [SerializeField] protected InputActionReference _lookInput;
    [SerializeField] protected float _rotationSpeed = 45f;

    void Update()
    {
        _characterController.Move(GetMoveDirection() * _speed * Time.deltaTime);
        var lookValues = _lookInput.action.ReadValue<Vector2>();
        transform.Rotate(0, lookValues.x * _rotationSpeed * Time.deltaTime, 0);
    }

    protected Vector3 GetMoveDirection()
    {
        var inputValues = _moveInput.action.ReadValue<Vector2>();
        var direction = transform.forward * inputValues.y + transform.right * inputValues.x;
        return direction.normalized;
    }
}
