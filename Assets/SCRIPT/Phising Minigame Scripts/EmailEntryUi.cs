using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EmailEntryUi : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI senderText;
    public TextMeshProUGUI subjectText;
    public Image background;

    [Header("Visual Feedback Colors")]
    public Color defaultColor = Color.white;
    public Color selectedColor = new Color(0.9f, 0.9f, 0.7f);
    public Color correctColor = new Color(0.7f, 1f, 0.7f);
    public Color incorrectColor = new Color(1f, 0.7f, 0.7f);

    [HideInInspector]
    public EmailData emailData { get; private set; }
    private PhishingUIManager uiManager;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSelect);
        if (background != null)
        {
            defaultColor = background.color;
        }
    }

    public void Setup(EmailData data, PhishingUIManager manager)
    {
        emailData = data;
        uiManager = manager;

        titleText.text = emailData.emailTitle;
        senderText.text = "From: " + emailData.emailSender;
        subjectText.text = "Subject: " + emailData.emailSubject;
    }

    private void OnSelect()
    {
        uiManager.SelectEmail(this);
    }

    public void SetSelected(bool isSelected)
    {
        if (background != null)
        {
            background.color = isSelected ? selectedColor : defaultColor;
        }
    }

    public void ShowFeedback(bool isCorrect)
    {
        if (background != null)
        {
            background.color = isCorrect ? correctColor : incorrectColor;
        }
        button.interactable = false;
    }
}
