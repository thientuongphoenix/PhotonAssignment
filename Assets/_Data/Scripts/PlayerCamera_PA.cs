using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera_PA : NetworkBehaviour, ISpawned
{
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance = 5f;
    [SerializeField] private float _orbitSpeed = 3f;
    [SerializeField] private float _minYAngle = -30f;
    [SerializeField] private float _maxYAngle = 60f;
    [SerializeField] private InputActionReference _lookInput;

    private float _yaw = 0f;
    private float _pitch = 20f;

    public override void Spawned() => _playerCamera.SetActive(HasStateAuthority);

    private void LateUpdate()
    {
        if (!_playerCamera.activeSelf) return;
        Vector2 look = _lookInput.action.ReadValue<Vector2>();
        _yaw += look.x * _orbitSpeed;
        _pitch -= look.y * _orbitSpeed;
        _pitch = Mathf.Clamp(_pitch, _minYAngle, _maxYAngle);

        if (_target != null)
        {
            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
            Vector3 offset = rotation * new Vector3(0, 0, -_distance);
            _playerCamera.transform.position = _target.position + offset + Vector3.up * 1.5f;
            _playerCamera.transform.LookAt(_target.position + Vector3.up * 1.5f);
        }
    }
}
