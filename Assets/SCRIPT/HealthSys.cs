using UnityEngine;
using UnityEngine.UI; // Jangan lupa tambahkan ini untuk UI

public class DataCenterHealth : MonoBehaviour
{
    public static DataCenterHealth instance;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public Slider healthBarSlider; // Referensi ke UI Slider untuk health bar

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBarUI();
    }

    // Fungsi ini akan dipanggil oleh skrip lain untuk mengurangi health
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Pastikan health tidak kurang dari 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Data Center terkena damage! Health tersisa: " + currentHealth);
        UpdateHealthBarUI();

        // Opsional: Tambahkan efek visual/suara saat terkena damage
        // ScreenShake.instance.Shake();
        // AlarmSound.Play();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHealthBarUI()
    {
        if (healthBarSlider != null)
        {
            // Nilai slider (0-1) = health saat ini / health maksimal
            healthBarSlider.value = currentHealth / maxHealth;
        }
    }

    private void GameOver()
    {
        Debug.LogError("GAME OVER! Health Data Center habis!");
        // Di sini kamu bisa memanggil GameManager untuk menampilkan layar Game Over
        Time.timeScale = 0f; // Contoh: menghentikan game
    }
}