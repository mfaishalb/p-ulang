using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // Pastikan ini ada!

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Button continueButton;

    // BARU: Referensi ke CanvasGroup
    private CanvasGroup dialogueCanvasGroup;

    // HAPUS: private Animator dialogueAnimator; // Ini sudah tidak kita pakai

    [Header("Animation Settings")]
    public float fadeDuration = 0.3f; // Durasi animasi fade

    void Awake()
    {
        instance = this;
        // BARU: Dapatkan komponen CanvasGroup
        dialogueCanvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        // Panelnya sudah kita set Alpha = 0, tapi kita juga matikan interaksinya
        dialogueCanvasGroup.alpha = 0f;
        dialogueCanvasGroup.interactable = false;

        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    public void ShowDialogue(DialogueData data)
    {
        if (data == null) return;

        portraitImage.sprite = data.characterPortrait;
        nameText.text = data.characterName;
        dialogueText.text = data.dialogueText;

        // dialoguePanel.SetActive(true); // Kita tidak pakai SetActive lagi

        Time.timeScale = 0f; // Jeda game

        // --- INI DIA KODE DOTWEEN-NYA ---
        // Animasikan Alpha dari 0 ke 1 selama 'fadeDuration'
        dialogueCanvasGroup.DOFade(1f, fadeDuration)
            .SetUpdate(true); // SetUpdate(true) penting agar animasi jalan saat Time.timeScale = 0

        // Aktifkan interaksi
        dialogueCanvasGroup.interactable = true;
    }

    public void OnContinueButtonClicked()
    {
        // --- ANIMASI HILANG DOTWEEN ---
        dialogueCanvasGroup.DOFade(0f, fadeDuration)
            .SetUpdate(true) // Tetap pakai SetUpdate(true)
            .OnComplete(HideDialogueComplete); // Panggil fungsi ini setelah animasi selesai
    }

    // Fungsi ini akan dipanggil otomatis setelah animasi fade-out selesai
    private void HideDialogueComplete()
    {
        Time.timeScale = 1f; // Lanjutkan game
        dialogueCanvasGroup.interactable = false;
        // dialoguePanel.SetActive(false); // Tidak perlu, biarkan aktif tapi transparan
    }
}