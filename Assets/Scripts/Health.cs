using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private float MaxHealth = 100;
    
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHealth { get; set; } = 100;

    public UnityEvent OnHealthChanged;

    public float HealthRate => NetworkedHealth / MaxHealth;

    private void HealthChanged()
    {
        print($"{gameObject.name} Health changed to {NetworkedHealth}");
        OnHealthChanged.Invoke();
    }

    [ContextMenu("Simulate deal damage")]
    public void SimulateDealDamage()
    {
        DealDamageRpc(1);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damage)
    {
        NetworkedHealth -= damage;
    }
}
