using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Email Details Panel")]
    public GameObject detailsPanel;
    public TMP_Text detailsSenderText;
    public TMP_Text detailsSubjectText;
    public TMP_Text detailsBodyText;

    private EmailEntryUi selectedEmailEntry;
    private Mission_PhishingEmail currentMission;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        inboxPanel.SetActive(false);
        if (detailsPanel != null) detailsPanel.SetActive(false);

        // Sambungkan listener ke fungsi yang akan kita buat di bawah
        phishingButton.onClick.AddListener(OnPhishingButton);
        legitimateButton.onClick.AddListener(OnLegitimateButton);
        closeButton.onClick.AddListener(HideInbox);
    }

    public void ShowInbox(List<EmailData> emails, Mission_PhishingEmail mission)
    {
        currentMission = mission;

        foreach (Transform child in emailGridContainer) Destroy(child.gameObject);
        foreach (EmailData email in emails)
        {
            GameObject emailObject = Instantiate(emailEntryPrefab, emailGridContainer);
            emailObject.GetComponent<EmailEntryUi>().Setup(email, this);
        }

        inboxPanel.SetActive(true);
        selectedEmailEntry = null;
        if (detailsPanel != null) detailsPanel.SetActive(false);

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
        ShowEmailDetails(emailEntry.emailData);
    }

    private void ShowEmailDetails(EmailData data)
    {
        if (detailsPanel == null) return;
        detailsPanel.SetActive(true);
        if (detailsSenderText != null) detailsSenderText.text = "From: " + data.emailSender;
        if (detailsSubjectText != null) detailsSubjectText.text = "Subject: " + data.emailSubject;
        if (detailsBodyText != null) detailsBodyText.text = data.emailBody;
    }

    // --- FUNGSI BARU YANG DITAMBAHKAN UNTUK MENGATASI ERROR ---
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

    private void CheckAnswer(bool playerChoiceIsPhishing)
    {
        if (selectedEmailEntry == null) return;

        bool isCorrect = selectedEmailEntry.emailData.isPhising == playerChoiceIsPhishing;
        selectedEmailEntry.ShowFeedback(isCorrect);

        if (isCorrect)
        {
            if (currentMission != null)
            {
                currentMission.OnCorrectAnswer();
            }
        }

        selectedEmailEntry = null;
    }

    public void HideInbox()
    {
        if (currentMission != null)
        {
            currentMission.OnInboxClosed();
        }
        inboxPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // --------------------------------------------------------
}