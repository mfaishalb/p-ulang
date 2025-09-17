using System.Collections.Generic;
using UnityEngine;

public class Mission_PhishingEmail : Mission
{
    [Header("Email Database")]
    public List<EmailData> allEmails; // Database semua email yang mungkin muncul
    public int emailsToShow = 4;
    public int requiredCorrectAnswers = 3;

    private int currentCorrectAnswers = 0;

    // Fungsi ini dipanggil saat player berinteraksi dengan terminal
    public override void StartMission()
    {
        // Reset counter setiap kali misi dimulai
        currentCorrectAnswers = 0;

        // --- Logika memilih email acak (dari skrip lamamu) ---
        List<EmailData> selectedEmails = new List<EmailData>();
        List<int> selectedEmailsIndex = new List<int>();

        if (allEmails != null && allEmails.Count > 0)
        {
            while (selectedEmailsIndex.Count < Mathf.Min(emailsToShow, allEmails.Count))
            {
                int randomIndex = Random.Range(0, allEmails.Count);
                if (!selectedEmailsIndex.Contains(randomIndex))
                {
                    selectedEmailsIndex.Add(randomIndex);
                    selectedEmails.Add(allEmails[randomIndex]);
                }
            }
        }

        // --- Panggil UI Manager ---
        if (PhishingUIManager.instance != null)
        {
            // Berikan UI Manager referensi ke 'misi' ini, bukan ke 'SabotageableObject'
            PhishingUIManager.instance.ShowInbox(selectedEmails, this);
        }
        else
        {
            Debug.LogWarning("PhishingUIManager instance is not set.");
        }
    }

    // Fungsi ini akan dipanggil oleh PhishingUIManager setiap kali player memilih jawaban benar
    public void OnCorrectAnswer()
    {
        currentCorrectAnswers++;
        Debug.Log("Jawaban benar! Progres: " + currentCorrectAnswers + "/" + requiredCorrectAnswers);

        // Cek apakah misi sudah selesai
        if (currentCorrectAnswers >= requiredCorrectAnswers)
        {
            Debug.Log("Misi Phising Selesai!");

            // Sembunyikan UI dan selesaikan sabotase
            if (PhishingUIManager.instance != null)
            {
                PhishingUIManager.instance.HideInbox();
            }
            owner.ResolveSabotage(); // Lapor ke 'owner' (SabotageableObject) bahwa misi beres
        }
    }

    public void OnInboxClosed()
    {
        Debug.Log("Pemain menutup inbox. Percobaan saat ini dibatalkan.");
        // Kita tidak perlu melakukan apa-apa di sini, karena kembalinya
        // Time.timeScale = 1f secara otomatis akan melanjutkan timer utama.
    }
}