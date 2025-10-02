using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    
    public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Button continueButton;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        // Sambungkan tombol continue untuk menyembunyikan dialog
        continueButton.onClick.AddListener(HideDialogue);
    }

    // Fungsi utama untuk menampilkan dialog
    public void ShowDialogue(DialogueData data)
    {
        if (data == null) return;

        // Isi semua elemen UI dengan data dari ScriptableObject
        portraitImage.sprite = data.characterPortrait;
        nameText.text = data.characterName;
        dialogueText.text = data.dialogueText;

        dialoguePanel.SetActive(true);
        
        Time.timeScale = 0f; // Jeda game saat dialog muncul
    }

    // Fungsi untuk menyembunyikan dialog
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        
        Time.timeScale = 1f; // Lanjutkan game
    }
}