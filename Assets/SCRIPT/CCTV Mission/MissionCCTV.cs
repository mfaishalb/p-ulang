using System.Collections.Generic;
using UnityEngine;

public class Mission_CCTV : Mission
{
    private List<CCTVCamera> sabotagedCameras;
    private int camerasToRepair = 3;
    private int camerasRepaired = 0;

    public override void StartMission()
    {
        // 1. Mulai sabotase dan dapatkan list CCTV yang rusak
        sabotagedCameras = CCTVManager.instance.TriggerSabotage(camerasToRepair, this);

        // 2. Tampilkan UI Laptop
        CCTV_UI_Controller.instance.ShowPanel();
        CCTV_UI_Controller.instance.UpdateStatus(CCTVManager.instance.allCameras);

        // 3. Reset counter perbaikan
        camerasRepaired = 0;
    }

    // Dipanggil oleh CCTVCamera saat satu perbaikan selesai
    public void OnCameraRepaired(CCTVCamera repairedCamera)
    {
        camerasRepaired++;
        Debug.Log("CCTV Diperbaiki: " + repairedCamera.roomName + ". Total diperbaiki: " + camerasRepaired + "/" + camerasToRepair);

        // Update UI lagi untuk menunjukkan CCTV sudah online
        CCTV_UI_Controller.instance.UpdateStatus(CCTVManager.instance.allCameras);

        // Cek apakah semua CCTV sudah diperbaiki
        if (camerasRepaired >= camerasToRepair)
        {
            Debug.Log("SEMUA CCTV TELAH DIPERBAIKI!");
            CCTV_UI_Controller.instance.HidePanel();
            owner.ResolveSabotage(); // Misi sabotase utama selesai!
        }
    }
}