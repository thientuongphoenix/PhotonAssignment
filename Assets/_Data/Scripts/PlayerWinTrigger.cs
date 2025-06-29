using UnityEngine;

public class PlayerWinTrigger : MonoBehaviour
{
    public CheckWin checkWin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinZone"))
        {
            if (checkWin == null)
            {
                checkWin = Object.FindAnyObjectByType<CheckWin>();
                if (checkWin == null)
                {
                    Debug.LogError("Không tìm thấy CheckWin");
                    return;
                }
            }
            Debug.Log("Player đã vào vùng thắng");
            checkWin.RpcRequestWin();
        }
    }
}
