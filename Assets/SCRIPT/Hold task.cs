using System.Reflection;
using UnityEngine;
using UnityEngine.UI; // Jangan lupa tambahkan ini untuk UI

public class Mission_HoldKey : Mission
{
    [Header("Settings")]
    public float holdDuration = 3f;
    public KeyCode interactionKey = KeyCode.F;

    [Header("UI")]
    [Tooltip("Drag UI Image dengan tipe Filled di sini")]
    public Image progressBar; // Untuk visual progress

    private float holdTimer = 0f;

    void Start()
    {
        // Pastikan progress bar disembunyikan di awal
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }
    }

    // Untuk misi jenis ini, StartMission() hanya mengaktifkan UI
    public override void StartMission()
    {
        Debug.Log("Misi Tahan Tombol Dimulai!");
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
        }
        holdTimer = 0f; // Reset timer setiap kali interaksi
    }

    // Logika utamanya ada di Update!
    void Update()
    {
        // Jika progress bar tidak aktif, jangan lakukan apa-apa
        if (progressBar == null || !progressBar.gameObject.activeSelf) return;

        // Cek jika player menahan tombol
        if (Input.GetKey(interactionKey))
        {
            holdTimer += Time.deltaTime;
            progressBar.fillAmount = holdTimer / holdDuration;

            if (holdTimer >= holdDuration)
            {
                Debug.Log("Misi Tahan Tombol Selesai!");
                progressBar.gameObject.SetActive(false);
                owner.ResolveSabotage(); // Beri tahu 'pemilik' bahwa misi sukses!
            }
        }
        // Jika tombol dilepas, reset progress
        else if (Input.GetKeyUp(interactionKey))
        {
            holdTimer = 0f;
            progressBar.fillAmount = 0f;
        }
    }
}