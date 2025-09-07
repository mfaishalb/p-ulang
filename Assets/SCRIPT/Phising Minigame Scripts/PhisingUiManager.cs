using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhishingUIManager : MonoBehaviour
{
    public static PhishingUIManager instance;

    [Header("UI Panel")]
    [Tooltip("The parent GameObject of the email inbox interface.")]
    public GameObject inboxPanel;

    [Header("UI Setup")]
    [Tooltip("The prefab for a single email entry in the list.")]
    public GameObject emailEntryPrefab;
    [Tooltip("The parent object where the email prefabs will be instantiated (e.g., a panel with a Grid Layout Group).")]
    public Transform emailGridContainer;

    [Header("Action Buttons")]
    [Tooltip("The button for marking an email as legitimate (e.g., the ? button).")]
    public Button legitimateButton;
    [Tooltip("The button for marking an email as phishing (e.g., the ? button).")]
    public Button phishingButton;
    [Tooltip("The button to close the inbox panel (e.g., the back arrow).")]
    public Button closeButton;

    private EmailEntryUi selectedEmailEntry;
    private List<EmailData> correctEmails = new();
    private EmailTerminal emailTerminal;

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
        closeButton.onClick.AddListener(OnCloseButton);
    }

    public void ShowInbox(List<EmailData> emails, EmailTerminal terminal)
    {
        emailTerminal = terminal;
        correctEmails.Clear();

        foreach (Transform child in emailGridContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (EmailData email in emails)
        {
            GameObject emailObject = Instantiate(emailEntryPrefab, emailGridContainer);
            emailObject.GetComponent<EmailEntryUi>().Setup(email, this);
        }

        inboxPanel.SetActive(true);
        selectedEmailEntry = null;

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

    private void CheckAnswer(bool playerChoiceIsPhishing)
    {
        //bool isCorrect = selectedEmailEntry.emailData.isPhising == playerChoiceIsPhishing;
        //selectedEmailEntry.ShowFeedback(isCorrect);
        if (selectedEmailEntry.emailData.isPhising == playerChoiceIsPhishing)
        {
            if(!correctEmails.Contains(selectedEmailEntry.emailData))
                correctEmails.Add(selectedEmailEntry.emailData);
            Debug.Log("Correct! Total correct: " + correctEmails.Count);
        }
        else
        {
            Debug.Log("Incorrect!");
        }

        selectedEmailEntry.SetSelected(false);
        selectedEmailEntry = null;
    }

    private void OnCloseButton()
    {
        if (emailTerminal != null && correctEmails.Count < emailTerminal.EmailCount)
        {
            Debug.Log("Ulangi");
        } else
        {
            Debug.Log("Selesai");
        }
        
        inboxPanel.SetActive(false);

        emailTerminal = null;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
