using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhishingUIManager : MonoBehaviour
{
    public static PhishingUIManager instance;

    [Header("UI Panel")]
    public GameObject inboxPanel;

    [Header("UI Setup")]
    public GameObject emailEntryPrefab;
    public Transform emailGridContainer;

    [Header("Action Buttons")]
    public Button legitimateButton;
    public Button phishingButton;
    public Button closeButton;

    private EmailEntryUi selectedEmailEntry;
    // VARIABEL DIUBAH: Sekarang kita menyimpan referensi ke misi, bukan terminal
    private Mission_PhishingEmail currentMission;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        inboxPanel.SetActive(false);
        phishingButton.onClick.AddListener(OnPhishingButton);
        legitimateButton.onClick.AddListener(OnLegitimateButton);
        closeButton.onClick.AddListener(HideInbox); // Tombol close sekarang hanya menutup panel
    }

    // FUNGSI DIUBAH: Parameter sekarang menerima Mission_PhishingEmail
    public void ShowInbox(List<EmailData> emails, Mission_PhishingEmail mission)
    {
        currentMission = mission;

        // Membersihkan email dari sesi sebelumnya
        foreach (Transform child in emailGridContainer)
        {
            Destroy(child.gameObject);
        }

        // Membuat entri email baru
        foreach (EmailData email in emails)
        {
            GameObject emailObject = Instantiate(emailEntryPrefab, emailGridContainer);
            emailObject.GetComponent<EmailEntryUi>().Setup(email, this);
        }

        inboxPanel.SetActive(true);
        selectedEmailEntry = null;

        // Jeda game dan tampilkan cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SelectEmail(EmailEntryUi emailEntry)
    {
        if (selectedEmailEntry != null)
        {
            selectedEmailEntry.SetSelected(false);
        }
        selectedEmailEntry = emailEntry;
        selectedEmailEntry.SetSelected(true);
    }

    private void OnPhishingButton()
    {
        if (selectedEmailEntry == null) return;
        CheckAnswer(true);
    }

    private void OnLegitimateButton()
    {
        if (selectedEmailEntry == null) return;
        CheckAnswer(false);
    }

    // LOGIKA DIUBAH TOTAL: Sekarang lebih jelas dan langsung memanggil misi
    private void CheckAnswer(bool playerChoiceIsPhishing)
    {
        if (selectedEmailEntry == null) return;

        // Cek apakah jawaban player sesuai dengan tipe email sebenarnya
        bool isCorrect = selectedEmailEntry.emailData.isPhising == playerChoiceIsPhishing;

        // Beri feedback visual di entri email (hijau jika benar, merah jika salah)
        selectedEmailEntry.ShowFeedback(isCorrect);

        if (isCorrect)
        {
            Debug.Log("Jawaban Benar!");
            // Panggil fungsi di misi untuk menambah progres
            if (currentMission != null)
            {
                currentMission.OnCorrectAnswer();
            }
        }
        else
        {
            Debug.Log("Jawaban Salah!");
            // Di sini kamu bisa menambahkan penalti, misal mengurangi waktu sabotase
        }

        selectedEmailEntry = null; // Reset pilihan
    }

    // FUNGSI DIUBAH: Sekarang hanya untuk menutup panel dan melanjutkan game
    public void HideInbox()
    {
        // PANGGIL FUNGSI DI MISI SEBELUM MENUTUP
        if (currentMission != null)
        {
            currentMission.OnInboxClosed();
        }

        inboxPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Close Button Pressed");
    }
}