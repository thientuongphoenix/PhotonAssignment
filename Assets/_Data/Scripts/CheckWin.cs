using UnityEngine;
using Fusion;
using TMPro;

public class CheckWin : NetworkBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TextMeshProUGUI _winText;

    void Start()
    {
        _winPanel.SetActive(false);
    }

    [Networked, OnChangedRender(nameof(OnWinChanged))]
    public bool IsWin { get; set; }

    [Networked, OnChangedRender(nameof(OnPlayerIDWin))]
    public string PlayerIDWin { get; set; }

    private void OnWinChanged()
    {
        _winPanel.SetActive(IsWin);
    }

    private void OnPlayerIDWin()
    {
        if (_winText != null && IsWin)
        {
            _winText.text = $"Player {PlayerIDWin} Win!!!";
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcRequestWin(string winnerId)
    {
        IsWin = true;
        PlayerIDWin = winnerId;
    }
}
