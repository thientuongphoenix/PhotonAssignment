using Fusion;
using UnityEngine;

public class RaycastAttack : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _damage = 10f;

    void Update()
    {
        if (!HasInputAuthority) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Attack();
        }
    }

    private void Attack()
    {
        var aimingRay = new Ray(_camera.position, _camera.forward);
        Debug.DrawRay(aimingRay.origin, aimingRay.direction * 100, Color.red, 3f);

        if (Physics.Raycast(aimingRay, out var hitInfo))
        {
            if (hitInfo.collider.TryGetComponent<Health>(out var health))
            {
                health.DealDamageRpc(_damage);
            }
        }
    }
}
