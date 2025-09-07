using UnityEngine;
using UnityEngine.UI;

// SabotageableObject adalah sebuah Interactable, tapi dengan fitur tambahan.
public class SabotageableObject : Interactable
{
    [Header("Sabotage Settings")]
    // Drag & drop GameObject tanda seru "!" ke sini di Inspector
    public GameObject sabotageIndicator;
    public string sabotageMessage = "Perbaiki kerusakan"; // Pesan saat disabotase

    private bool isSabotaged = false;

    [Header("Minigame Settings")]
    public string minigameSceneName;

    // Kita gunakan Start dari parent, tapi tambahkan logika kita sendiri.
    void Start()
    {
        // Pastikan outline didapatkan dari kelas dasar (Interactable)
        outline = GetComponent<Outline>();
        DisableOutline(); // Ini fungsi dari Interactable

        // Logika tambahan khusus untuk SabotageableObject
        if (sabotageIndicator != null)
        {
            sabotageIndicator.SetActive(false);
        }
    }

    // Fungsi ini akan dipanggil oleh SabotageManager
    public void ActivateSabotage()
    {
        isSabotaged = true;
        message = sabotageMessage; // Ganti pesan interaksi saat disabotase
        if (sabotageIndicator != null)
        {
            sabotageIndicator.SetActive(true);
        }
    }

    // Kita 'menimpa' fungsi Interact() dari parent dengan logika baru
    public override void Interact()
    {
        if (!isSabotaged)
        {
            Debug.Log(gameObject.name + " sedang tidak rusak.");
            return;
        }

        // JANGAN panggil Resolve() di sini lagi.
        // Panggil GameManager untuk memulai minigame.
        if (GameManager.instance != null && !string.IsNullOrEmpty(minigameSceneName))
        {
            GameManager.instance.StartMinigame(minigameSceneName, this);
        }
        else
        {
            Debug.LogError("GameManager tidak ditemukan atau nama scene minigame kosong!");
        }
    }

    // Ubah nama Resolve() agar lebih jelas dan buat jadi public
    // Fungsi ini sekarang akan dipanggil oleh GameManager
    public void ResolveSabotage()
    {
        isSabotaged = false;
        if (sabotageIndicator != null)
        {
            sabotageIndicator.SetActive(false);
        }

        // LAPOR KEMBALI KE MANAJER SABOTASE BAHWA SEMUA BERES!
        if (SabotageManager.instance != null)
        {
            SabotageManager.instance.SabotageResolved();
        }
    }
}