using Fusion;
using UnityEngine;

public class NetworkMenu : MonoBehaviour
{
    [SerializeField] private FusionBootstrap _bootstrap;

    public void OnQuickStartButtonClicked()
    {
        _bootstrap.StartSharedClient();
        gameObject.SetActive(false);
    }
}
