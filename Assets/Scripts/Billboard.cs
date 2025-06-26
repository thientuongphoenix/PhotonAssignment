using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform _mainCamera;

    void Start() => _mainCamera = Camera.main.transform;

    // Update is called once per frame
    void Update() => transform.forward = _mainCamera.forward;
}
