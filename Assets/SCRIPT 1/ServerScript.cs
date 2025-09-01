// 8/20/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerScript : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad; // Nama scene yang ingin dimuat

    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log($"Loading scene: {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name is not set in ServerScript!");
        }
    }

    public void StartMiniGame()
    {
        // Simpan posisi dan rotasi pemain
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            GameStateManager.Instance.PlayerPosition = player.transform.position;
            GameStateManager.Instance.PlayerRotation = player.transform.rotation;
        }

        // Simpan status objek penting
        SaveSceneState();

        // Pindah ke scene mini-game
        SceneManager.LoadScene(sceneToLoad);
    }

    private void SaveSceneState()
    {
        
    }
}
