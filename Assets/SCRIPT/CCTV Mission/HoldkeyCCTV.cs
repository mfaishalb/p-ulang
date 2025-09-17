using UnityEngine;
using UnityEngine.UI;

public class CCTV_RepairMission : Mission
{
    [Header("Repair Settings")]
    public float repairDuration = 5f;
    public KeyCode repairKey = KeyCode.F;

    [Header("References")]
    public Animator targetAnimator;
    public Image progressBar;

    private CCTVCamera parentCamera;
    private float repairTimer = 0f;
    private bool isPlayerInRange = false;

    void Awake()
    {
        parentCamera = GetComponent<CCTVCamera>();
        // DEBUG: Memastikan skrip ini sadar siapa 'induk'-nya.
        if (parentCamera != null)
        {
            Debug.Log(gameObject.name + ": Awake() - Berhasil menemukan parent CCTVCamera.");
        }
        else
        {
            Debug.LogError(gameObject.name + ": Awake() - GAGAL menemukan komponen CCTVCamera!");
        }
    }

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }
    }

    public override void StartMission()
    {
        // DEBUG: Dipanggil oleh CCTVCamera saat sabotase dimulai.
        Debug.Log(gameObject.name + ": StartMission() - Misi diaktifkan oleh CCTVCamera.");
        repairTimer = 0f;
        isPlayerInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // DEBUG: Konfirmasi player masuk zona.
            Debug.Log(gameObject.name + ": OnTriggerEnter - Player MASUK zona. isPlayerInRange sekarang: " + isPlayerInRange);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // DEBUG: Konfirmasi player keluar zona.
            Debug.Log(gameObject.name + ": OnTriggerExit - Player KELUAR zona. isPlayerInRange sekarang: " + isPlayerInRange);
        }
    }

    void Update()
    {
        if (!this.enabled) return;

        // Cek kondisi utama
        if (isPlayerInRange && Input.GetKey(repairKey))
        {
            // DEBUG: Kondisi utama terpenuhi, perbaikan seharusnya berjalan.
            Debug.Log(gameObject.name + ": Update() - Player di dalam zona DAN menahan tombol F. Timer berjalan.");

            repairTimer += Time.deltaTime;

            if (progressBar != null)
            {
                progressBar.gameObject.SetActive(true);
                progressBar.fillAmount = repairTimer / repairDuration;
            }
            if (targetAnimator != null) targetAnimator.SetBool("isHolding", true);

            if (repairTimer >= repairDuration)
            {
                // DEBUG: Perbaikan selesai.
                Debug.LogWarning(gameObject.name + ": Update() - Perbaikan SELESAI. Memanggil ReportRepairComplete().");

                if (progressBar != null)
                {
                    progressBar.gameObject.SetActive(false);
                }

                // BARIS BARU: Hentikan animasi
                if (targetAnimator != null)
                {
                    targetAnimator.SetBool("isHolding", false);
                }

                parentCamera.ReportRepairComplete();
                this.enabled = false;
            }
        }
        else
        {
            // Jika salah satu kondisi tidak terpenuhi, batalkan.
            if (repairTimer > 0f) // Hanya reset jika memang sedang berjalan
            {
                // DEBUG: Memberi tahu kenapa perbaikan berhenti.
                if (!isPlayerInRange)
                {
                    Debug.Log(gameObject.name + ": Update() - Perbaikan dibatalkan karena Player KELUAR ZONA.");
                }
                else if (Input.GetKeyUp(repairKey))
                {
                    Debug.Log(gameObject.name + ": Update() - Perbaikan dibatalkan karena tombol F DILEPAS.");
                }

                repairTimer = 0f;
                if (progressBar != null)
                {
                    progressBar.fillAmount = 0f;
                    progressBar.gameObject.SetActive(false);
                }
                if (targetAnimator != null) targetAnimator.SetBool("isHolding", false);
            }
        }
    }
}