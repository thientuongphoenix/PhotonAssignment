using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_PA : MonoBehaviour
{
    [SerializeField] protected CharacterController _characterController;
    [SerializeField] protected InputActionReference _moveInput;

    void Update()
    {
        var inputValues = _moveInput.action.ReadValue<Vector2>();
        var direction = transform.forward * inputValues.y + transform.right * inputValues.x;
        direction = direction.normalized;
    }
}
