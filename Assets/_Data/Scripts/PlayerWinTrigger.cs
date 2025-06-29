using Fusion;
using UnityEngine;

public class PlayerWinTrigger : NetworkBehaviour, ISpawned
{
    public CheckWin checkWin;
    public PlayerRef playerRef;
    public PlayerSpawner playerSpawner;

    public override void Spawned()
    {
        base.Spawned();
        if (playerSpawner == null)
        {
            playerSpawner = FindObjectOfType<PlayerSpawner>();
            if (playerSpawner == null)
            {
                Debug.LogError("Không tìm thấy PlayerSpawner");
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinZone"))
        {
            if (checkWin == null)
            {
                checkWin = FindObjectOfType<CheckWin>();
                if (checkWin == null)
                {
                    Debug.LogError("Không tìm thấy CheckWin");
                    return;
                }
            }
            Debug.Log("Player đã vào vùng thắng");
            checkWin.RpcRequestWin(GetPlayerID());
        }
    }

    public string GetPlayerID()
    {
        return playerSpawner.playerID;
    }
}
