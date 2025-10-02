using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public static PlayerEnergy instance;

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    [SerializeField] private float currentEnergy;

    [Header("Time-Based Depletion")]
    public float depletionInterval = 30f;
    public float depletionAmount = 10f;
    private float depletionTimer;

    // --- DIUBAH: Referensi sekarang ke PlayerController ---
    [Header("Low Energy Consequences")]
    [Tooltip("Referensi ke skrip pergerakan player")]
    public PlayerController playerController; // Menggunakan nama skrip aslimu
    [Tooltip("Batas energi dianggap rendah")]
    public float lowEnergyThreshold = 25f;
    [Tooltip("Pengali kecepatan saat energi rendah (misal: 0.5 untuk 50% kecepatan)")]
    public float slowSpeedMultiplier = 0.5f;
    private bool isSlowed = false;
    // ----------------------------------------------------

    [Header("UI")]
    public Slider energyBarSlider;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentEnergy = maxEnergy;
        depletionTimer = depletionInterval;
        UpdateEnergyBarUI();

        // DIUBAH: Pastikan playerController sudah di-assign
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        depletionTimer -= Time.deltaTime;
        if (depletionTimer <= 0)
        {
            DepleteEnergy(depletionAmount);
            depletionTimer = depletionInterval;
        }
    }

    public void DepleteEnergy(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        UpdateEnergyBarUI();
        CheckEnergyConsequences();
    }

    public void RestoreEnergy()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyBarUI();
        CheckEnergyConsequences();
    }

    private void UpdateEnergyBarUI()
    {
        if (energyBarSlider != null)
        {
            energyBarSlider.value = currentEnergy / maxEnergy;
        }
    }

    private void CheckEnergyConsequences()
    {
        if (playerController == null) return;

        // Cek jika energi di bawah batas DAN player belum dalam kondisi lambat
        if (currentEnergy <= lowEnergyThreshold && !isSlowed)
        {
            isSlowed = true;
            // DIUBAH: Panggil fungsi di PlayerController
            playerController.SetSpeedMultiplier(slowSpeedMultiplier);
            Debug.Log("Energi rendah, player melambat!");
        }
        // Cek jika energi sudah di atas batas DAN player masih dalam kondisi lambat
        else if (currentEnergy > lowEnergyThreshold && isSlowed)
        {
            isSlowed = false;
            // DIUBAH: Panggil fungsi di PlayerController
            playerController.SetSpeedMultiplier(1f); // Kembalikan ke kecepatan normal (100%)
            Debug.Log("Energi cukup, kecepatan kembali normal.");
        }
    }
}