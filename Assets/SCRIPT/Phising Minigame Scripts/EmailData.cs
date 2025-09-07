using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmailData", menuName = "Email/New Email")]
public class EmailData : ScriptableObject
{
    [Tooltip("Email Title")]
    public string emailTitle;

    [Tooltip("Email Sender")]
    public string emailSender;

    [Tooltip("Email Subject")]
    public string emailSubject;

    [Header("Phising Status")]
    public bool isPhising;
}
