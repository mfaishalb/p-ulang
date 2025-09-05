using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main Instance;

    public int switchCount;
    public GameObject winText;
    private int onCount = 0;
    private bool isWon = false;

    private void Awake()
    {
        Instance = this;
    }

    // FUNGSI BARU YANG DITAMBAHKAN
    void Start()
    {
        // Pastikan display kedua aktif saat scene ini dimuat
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }
    }

    public void SwitchChange(int points)
    {
        if (isWon) return;

        onCount = onCount + points;
        if (onCount == switchCount)
        {
            isWon = true;
            StartCoroutine(WinSequence());
        }
    }

    private IEnumerator WinSequence()
    {
        winText.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);

        if (GameManager.instance != null)
        {
            GameManager.instance.EndMinigame(true);
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}