using UnityEngine;
using TMPro;
using Fusion;

public class PlayerNameText : NetworkBehaviour, ISpawned
{
    [SerializeField] private TextMeshProUGUI nameText;

    [Networked, OnChangedRender(nameof(LoadPlayerName))]
    public string NetworkedName { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        LoadPlayerName();
    }

    private void LoadPlayerName() => nameText.text = NetworkedName;

    public void SetPlayerName(PlayerRef playerName)
    {
        if (HasStateAuthority)
        {
            NetworkedName = "Player " + playerName.PlayerId.ToString();
        }
    }
} 