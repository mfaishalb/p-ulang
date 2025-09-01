// 8/20/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    public string lastSabotageTarget;
    public static GameStateManager Instance { get; private set; }

    // Data yang disimpan
    public Vector3 PlayerPosition { get; set; }
    public Quaternion PlayerRotation { get; set; }
    public Dictionary<string, bool> ObjectStates { get; private set; } = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Fungsi untuk menyimpan status objek
    public void SaveObjectState(string objectId, bool state)
    {
        if (ObjectStates.ContainsKey(objectId))
        {
            ObjectStates[objectId] = state;
        }
        else
        {
            ObjectStates.Add(objectId, state);
        }
    }

    // Fungsi untuk mengambil status objek
    public bool GetObjectState(string objectId)
    {
        return ObjectStates.ContainsKey(objectId) && ObjectStates[objectId];
    }
}
