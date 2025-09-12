using UnityEngine;

public class SabotageableObject : Interactable
{
    [Header("Sabotage Settings")]
    public GameObject sabotageIndicator;
    public string sabotageMessage = "Perbaiki kerusakan";

    private bool isSabotaged = false;
    private Mission attachedMission; // Variabel untuk menyimpan 'kaset' misinya

    void Awake() // Ganti Start jadi Awake agar dieksekusi lebih dulu
    {
        outline = GetComponent<Outline>();
        DisableOutline();

        if (sabotageIndicator != null)
            sabotageIndicator.SetActive(false);

        // Cari 'kaset' misi yang terpasang pada objek ini
        attachedMission = GetComponent<Mission>();
        if (attachedMission != null)
        {
            // Inisialisasi misi dan beritahu siapa pemiliknya
            attachedMission.Initialize(this);
        }
        else
        {
            Debug.LogError("Objek " + gameObject.name + " bisa disabotase tapi tidak punya skrip Misi!");
        }
    }

    public void ActivateSabotage()
    {
        isSabotaged = true;
        message = sabotageMessage;
        if (sabotageIndicator != null)
            sabotageIndicator.SetActive(true);
    }

    public override void Interact()
    {
        if (!isSabotaged) return;

        // Cukup jalankan misi yang terpasang!
        if (attachedMission != null)
        {
            attachedMission.StartMission();
        }
    }

    // Fungsi ini dipanggil oleh Misi atau GameManager saat sabotase selesai
    public void ResolveSabotage()
    {
        isSabotaged = false;
        if (sabotageIndicator != null)
            sabotageIndicator.SetActive(false);

        if (SabotageManager.instance != null)
            SabotageManager.instance.SabotageResolved();
    }
}