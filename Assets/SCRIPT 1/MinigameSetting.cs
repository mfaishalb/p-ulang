using UnityEngine;

public class MinigameController : MonoBehaviour
{
    // Panggil fungsi ini saat minigame berhasil diselesaikan
    // Kamu bisa hubungkan ini ke sebuah tombol 'Selesai' di UI
    public void WinMinigame()
    {
        // Beri tahu GameManager bahwa kita menang
        if (GameManager.instance != null)
        {
            GameManager.instance.EndMinigame(true);
        }
    }

    // Panggil fungsi ini jika pemain gagal atau keluar
    public void LoseMinigame()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.EndMinigame(false);
        }
    }
}