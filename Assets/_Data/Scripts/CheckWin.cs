using UnityEngine;
using Fusion;

public class CheckWin : NetworkBehaviour
{
    [SerializeField] private GameObject _winPanel;

    void Start()
    {
        _winPanel.SetActive(false);
    }

    [Networked, OnChangedRender(nameof(OnWinChanged))]
    public bool IsWin { get; set; }

    private void OnWinChanged()
    {
        _winPanel.SetActive(IsWin);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcRequestWin()
    {
        IsWin = true;
    }
}
