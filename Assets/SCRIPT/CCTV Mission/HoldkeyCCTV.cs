using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCTV_RepairMission : Mission
{
    [Header("Repair Settings")]
    public float repairDuration = 5f;
    public KeyCode repairKey = KeyCode.F;

    // --- REFERENSI UI DIUBAH ---
    [Header("UI References")]
    [Tooltip("Background/bingkai progress bar")]
    public GameObject progressBarEmpty;
    [Tooltip("Image yang akan diisi (wajib tipe Filled)")]
    public Image progressBarFilling;
    [Tooltip("Tanda visual saat misi selesai")]
    public GameObject progressBarCompleted;
    // -------------------------

    [Header("References")]
    public Animator targetAnimator;

    private CCTVCamera parentCamera;
    private float repairTimer = 0f;
    private bool isPlayerInRange = false;

    void Awake()
    {
        parentCamera = GetComponent<CCTVCamera>();
        if (parentCamera == null)
        {
            Debug.LogError(gameObject.name + ": Awake() - GAGAL menemukan komponen CCTVCamera!");
        }
    }

    void Start()
    {
        // DIUBAH: Pastikan semua bagian progress bar nonaktif di awal
        if (progressBarEmpty != null) progressBarEmpty.SetActive(false);
        if (progressBarFilling != null) progressBarFilling.gameObject.SetActive(false);
        if (progressBarCompleted != null) progressBarCompleted.SetActive(false);
    }

    public override void StartMission()
    {
        repairTimer = 0f;
        isPlayerInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (!this.enabled) return;

        if (isPlayerInRange && Input.GetKey(repairKey))
        {
            // DIUBAH: Tampilkan UI progress bar saat perbaikan dimulai
            if (progressBarEmpty != null) progressBarEmpty.SetActive(true);
            if (progressBarFilling != null) progressBarFilling.gameObject.SetActive(true);

            repairTimer += Time.deltaTime;
            if (progressBarFilling != null) progressBarFilling.fillAmount = repairTimer / repairDuration;
            if (targetAnimator != null) targetAnimator.SetBool("isHolding", true);

            if (repairTimer >= repairDuration)
            {
                // DIUBAH: Panggil coroutine untuk sekuens "Selesai"
                StartCoroutine(CompleteSequence());
                this.enabled = false;
            }
        }
        else
        {
            // Jika tombol F dilepas atau player keluar zona, batalkan
            CancelRepair();
        }
    }

    // NAMA FUNGSI DIUBAH agar lebih jelas
    private void CancelRepair()
    {
        repairTimer = 0f;

        // DIUBAH: Pastikan semua UI disembunyikan saat batal
        if (progressBarEmpty != null) progressBarEmpty.SetActive(false);
        if (progressBarFilling != null)
        {
            progressBarFilling.fillAmount = 0f;
            progressBarFilling.gameObject.SetActive(false);
        }
        if (targetAnimator != null) targetAnimator.SetBool("isHolding", false);
    }

    // FUNGSI BARU: Mengatur urutan tampilan saat misi selesai
    private IEnumerator CompleteSequence()
    {
        // 1. Sembunyikan progress bar yang sedang berjalan
        if (progressBarEmpty != null) progressBarEmpty.SetActive(false);
        if (progressBarFilling != null) progressBarFilling.gameObject.SetActive(false);
        if (targetAnimator != null) targetAnimator.SetBool("isHolding", false);

        // 2. Tampilkan progress bar "Completed"
        if (progressBarCompleted != null) progressBarCompleted.SetActive(true);

        // 3. Tunggu selama 2 detik
        yield return new WaitForSeconds(2f);

        // 4. Sembunyikan progress bar "Completed"
        if (progressBarCompleted != null) progressBarCompleted.SetActive(false);

        // 5. Baru laporkan bahwa perbaikan sudah selesai
        parentCamera.ReportRepairComplete();
    }
}