using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Dibutuhkan untuk 'OrderBy'

public class CCTVManager : MonoBehaviour
{
    public static CCTVManager instance;
    public List<CCTVCamera> allCameras; // Masukkan semua 9 CCTV ke sini

    void Awake()
    {
        instance = this;
    }

    // Fungsi untuk memulai sabotase
    public List<CCTVCamera> TriggerSabotage(int count, Mission_CCTV missionController)
    {
        // Acak urutan list kamera dan ambil 3 pertama
        List<CCTVCamera> camerasToSabotage = allCameras.OrderBy(x => Random.value).Take(count).ToList();

        foreach (var cam in camerasToSabotage)
        {
            cam.GoOffline(missionController);
        }

        Debug.Log("CCTV disabotase: " + string.Join(", ", camerasToSabotage.Select(c => c.roomName)));
        return camerasToSabotage;
    }
}