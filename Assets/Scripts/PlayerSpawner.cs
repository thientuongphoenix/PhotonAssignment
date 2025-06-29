using UnityEngine;
using Fusion;
using System;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    public string playerID;

    public void PlayerJoined(PlayerRef player)
    {
        playerID = player.PlayerId.ToString();
        print("Player joined: " + player.PlayerId);
        if (player == Runner.LocalPlayer)
        {
            int locationIndex = (player.PlayerId - 1) % _spawnPoints.Length;
            var spawnPoint = _spawnPoints[locationIndex];
            var playerObj = Runner.Spawn(_playerPrefab, spawnPoint.position, spawnPoint.rotation, player);
            if (playerObj != null && player == Runner.LocalPlayer)
            {
                var nameText = playerObj.GetComponentInChildren<PlayerNameText>();
                if (nameText != null)
                    nameText.SetPlayerName(player);
            }
        }
    }
}
