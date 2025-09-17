using UnityEngine;
using UnityEngine.UI;

public class Mission_HoldKey : Mission
{
    [Header("Settings")]
    public float holdDuration = 3f;
    public KeyCode interactionKey = KeyCode.F;

    [Header("References")]
    [Tooltip("Animator yang akan memutar animasi perbaikan")]
    public Animator targetAnimator; // BARIS BARU: Referensi ke animator
    [Tooltip("Drag UI Image dengan tipe Filled di sini")]
    public Image progressBar;

    private float holdTimer = 0f;
    private bool isCurrentlyHolding = false; // BARIS BARU: Untuk melacak status hold

    private CCTVCamera parentCamera;

    void Awake()
    {
        parentCamera = GetComponent<CCTVCamera>();
    }
    void Start()
    {
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }

        // BARIS BARU: Pastikan animator ada
        if (targetAnimator == null)
        {
            Debug.LogWarning("Target Animator belum di-assign di " + gameObject.name);
        }
    }

    public override void StartMission()
    {
        Debug.Log("Misi Tahan Tombol Dimulai!");
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
        }
        holdTimer = 0f;
        isCurrentlyHolding = false; // Reset status hold
    }

    void Update()

    {
        if (!this.enabled) return;
        if (progressBar == null || !progressBar.gameObject.activeSelf) return;

        // Saat tombol F ditahan
        if (Input.GetKey(interactionKey))
        {
            // Set parameter animator ke true (HANYA jika belum di-set)
            if (!isCurrentlyHolding && targetAnimator != null)
            {
                isCurrentlyHolding = true;
                targetAnimator.SetBool("isHolding", true); // BARIS BARU: Mulai animasi
            }

            holdTimer += Time.deltaTime;
            progressBar.fillAmount = holdTimer / holdDuration;

            if (holdTimer >= holdDuration)
            {
                Debug.Log("Misi Tahan Tombol Selesai!");
                progressBar.gameObject.SetActive(false);

                // BARIS BARU: Hentikan animasi sebelum resolve
                if (targetAnimator != null)
                {
                    targetAnimator.SetBool("isHolding", false);
                }

                parentCamera.ReportRepairComplete();
                this.enabled = false;
            }
        }
        // Saat tombol F dilepas (cancel)
        else if (Input.GetKeyUp(interactionKey))
        {
            // Set parameter animator ke false
            if (isCurrentlyHolding && targetAnimator != null)
            {
                isCurrentlyHolding = false;
                targetAnimator.SetBool("isHolding", false); // BARIS BARU: Hentikan/balikkan animasi
            }

            holdTimer = 0f;
            progressBar.fillAmount = 0f;
        }
    }
}