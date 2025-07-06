using UnityEngine;
using Fusion;
using System;
using System.Linq;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] public TMPro.TextMeshProUGUI waitingText;
    public string playerID;
    // Biến đếm số lượng player đã join
    public static int playerCount = 0;

    private void Awake()
    {
        if (waitingText != null)
            waitingText.gameObject.SetActive(false);
    }

    public void PlayerJoined(PlayerRef player)
    {
        // Tăng biến đếm mỗi khi có player mới join
        playerCount++;
        if (playerCount == 2)
        {
            Debug.Log("2 player joined");
        }
        Debug.Log($"Số lượng player đã join: {playerCount}");
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

        if (waitingText != null)
        {
            if (playerCount < 2)
                waitingText.gameObject.SetActive(true);
            else
                waitingText.gameObject.SetActive(false);
        }

        // if (Runner.IsServer && Runner.ActivePlayers.Count() == 2)
        // {
        //     var allPlayers = FindObjectsOfType<PlayerMovement_PA>();
        //     foreach (var p in allPlayers)
        //     {
        //         p.RpcEnableMovement();
        //     }
        // }
    }

    
}
