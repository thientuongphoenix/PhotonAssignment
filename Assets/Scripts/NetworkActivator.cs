using Fusion;
using UnityEngine;

public class NetworkActivator : NetworkBehaviour
{
    [SerializeField] private GameObject[] _playerObjects;
    [SerializeField] private GameObject[] _enemyObjects;

    public override void Spawned()
    {
        base.Spawned();
        bool isPlayer = HasInputAuthority;
        foreach (var obj in _playerObjects)
        {
            obj.SetActive(isPlayer);
        }
        foreach (var obj in _enemyObjects)
        {
            obj.SetActive(!isPlayer);
        }
    }
}
