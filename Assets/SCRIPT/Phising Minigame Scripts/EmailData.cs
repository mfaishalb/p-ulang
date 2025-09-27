using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmailData", menuName = "Email/New Email")]
public class EmailData : ScriptableObject
{
    [Tooltip("Judul utama email di daftar")]
    public string emailTitle;

    [Tooltip("Pengirim email")]
    public string emailSender;

    [Tooltip("Subjek email")]
    public string emailSubject;

    [Tooltip("Isi lengkap email yang akan muncul di panel detail")]
    [TextArea(5, 10)] // Ini membuat field di Inspector jadi lebih besar dan mudah diisi
    public string emailBody; // <-- BARIS INI DITAMBAHKAN

    [Header("Phising Status")]
    [Tooltip("Centang jika ini adalah email phising")]
    public bool isPhising;
}