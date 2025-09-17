using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageManager : MonoBehaviour
{
    public static SabotageManager instance;

    // Masukkan semua objek yang bisa disabotase ke list ini melalui Inspector
    public List<SabotageableObject> sabotageableObjects;

    [Header("Timing Settings")]
    public float timeBetweenSabotages = 20f; // Waktu jeda antar sabotase (detik)
    public float sabotageDuration = 30f;   // Waktu untuk memperbaiki sebelum Game Over (detik)

    private bool isSabotageActive = false;
    private float sabotageTimer;
    private float timeUntilNextSabotage;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Mulai hitung mundur menuju sabotase pertama
        timeUntilNextSabotage = timeBetweenSabotages;
    }

    void Update()
    {
        if (isSabotageActive)
        {
            // Jika ada sabotase aktif, jalankan timer Game Over
            sabotageTimer -= Time.deltaTime;
            //Debug.Log("Waktu tersisa: " + sabotageTimer); // Untuk debug

            if (sabotageTimer <= 0)
            {
                GameOver();
                isSabotageActive = false; // Hentikan proses update
            }
        }
        else
        {
            // Jika tidak ada sabotase, hitung mundur ke sabotase berikutnya
            timeUntilNextSabotage -= Time.deltaTime;
            if (timeUntilNextSabotage <= 0)
            {
                StartNewSabotage();
            }
        }
    }

    void StartNewSabotage()
    {
        if (sabotageableObjects.Count == 0) return; // Jangan lakukan apa-apa jika list kosong

        isSabotageActive = true;

        // Pilih objek random dari list
        int randomIndex = Random.Range(0, sabotageableObjects.Count);
        SabotageableObject target = sabotageableObjects[randomIndex];

        // Aktifkan sabotase pada objek tersebut
        target.ActivateSabotage();
        Debug.Log("Sabotase dimulai pada: " + target.gameObject.name);

        // Reset timer Game Over
        sabotageTimer = sabotageDuration;
    }

    // Fungsi ini akan dipanggil oleh objek yang sudah diperbaiki
    public void SabotageResolved()
    {
        Debug.Log("Sabotase berhasil diperbaiki!");
        isSabotageActive = false;
        // Reset timer untuk sabotase berikutnya
        timeUntilNextSabotage = timeBetweenSabotages;
    }

    void GameOver()
    {
        Debug.Log("GAME OVER! Waktu habis.");
        // Di sini kamu bisa menambahkan logika untuk menampilkan layar Game Over, dll.
        Time.timeScale = 0; // Contoh: memberhentikan game
    }
}