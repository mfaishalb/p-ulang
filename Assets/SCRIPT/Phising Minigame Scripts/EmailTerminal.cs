using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailTerminal : MonoBehaviour, Iinteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;


    [Header("Email Content")]
    public List<EmailData> emails;
    public int EmailCount = 4;

    public bool Interact(Interactor interactor)
    {
        List<int> selectedEmailsIndex = new();
        List<EmailData> selectedEmails = new();

        if (emails != null && emails.Count > 0)
        {
            while (selectedEmailsIndex.Count < Mathf.Min(EmailCount, emails.Count))
            {
                int randomIndex = UnityEngine.Random.Range(0, emails.Count);
                if (!selectedEmailsIndex.Contains(randomIndex))
                {
                    selectedEmailsIndex.Add(randomIndex);
                    selectedEmails.Add(emails[randomIndex]);
                }
            }
        }

        if (PhishingUIManager.instance != null)
        {
            PhishingUIManager.instance.ShowInbox(selectedEmails, this);
            return true;
        }
        else
        {
            Debug.LogWarning("PhishingUIManager instance is not set.");
            return false;
        }
    }
}
