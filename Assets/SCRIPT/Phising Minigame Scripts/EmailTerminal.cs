using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailTerminal : SabotageableObject
{
    [Header("Email Content")]
    public List<EmailData> emails;
    public int EmailCount = 4;

    public override void Interact()
    {
        if (!isSabotaged) return;

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
        }
        else
        {
            Debug.LogWarning("PhishingUIManager instance is not set.");
        }
    }
}
