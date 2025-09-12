using UnityEngine;

public class Mission_LoadScene : Mission
{
    [Tooltip("Nama scene minigame yang akan dimuat")]
    public string minigameSceneName;

    public override void StartMission()
    {
        if (GameManager.instance != null && !string.IsNullOrEmpty(minigameSceneName))
        {
            // Panggil GameManager, berikan nama scene dan siapa 'pemilik' misinya
            GameManager.instance.StartMinigame(minigameSceneName, owner);
        }
        else
        {
            Debug.LogError("GameManager tidak ditemukan atau nama scene minigame kosong!");
        }
    }
}