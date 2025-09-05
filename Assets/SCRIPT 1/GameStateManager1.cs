using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // DRAG & DROP OBJEK-OBJEK INI DARI SCENE UTAMA KE INSPECTOR
    [Header("Main Scene Objects")]
    public GameObject playerObject;
    public GameObject mainCanvas;
    public GameObject mainCanvas1;// Canvas utama yang berisi HUD, dll
    public Camera mainCamera;

    private SabotageableObject currentSabotageSource;
    private string currentMinigameScene;

    void Awake()
    {
        // ... (kode Awake tetap sama) ...
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void StartMinigame(string sceneName, SabotageableObject source)
    {
        this.currentSabotageSource = source;
        this.currentMinigameScene = sceneName;

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // SEMBUNYIKAN SEMUA ELEMEN SCENE UTAMA
        if (playerObject != null) playerObject.SetActive(false);
        if (mainCanvas != null) mainCanvas.SetActive(false);
        if (mainCanvas != null) mainCanvas1.SetActive(false);
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);

        // Kita tidak perlu Time.timeScale = 0f lagi karena playernya sudah nonaktif
    }

    public void EndMinigame(bool wasSuccessful)
    {
        SceneManager.UnloadSceneAsync(currentMinigameScene);

        // TAMPILKAN KEMBALI SEMUA ELEMEN SCENE UTAMA
        if (playerObject != null) playerObject.SetActive(true);
        if (mainCanvas != null) mainCanvas.SetActive(true);
        if (mainCanvas != null) mainCanvas1.SetActive(true);
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);

        if (wasSuccessful && currentSabotageSource != null)
        {
            currentSabotageSource.ResolveSabotage();
        }
        
        currentSabotageSource = null;
    }
}