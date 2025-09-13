using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CCTV_UI_Controller : MonoBehaviour
{
    public static CCTV_UI_Controller instance;

    public GameObject cctvPanel; // Drag CCTV_Panel utama ke sini
    public List<CCTV_Status_UI> statusDisplays; // List dari UI status

    void Awake()
    {
        instance = this;
    }

    public void ShowPanel()
    {
        cctvPanel.SetActive(true);
    }


    public void HidePanel()
    {
        cctvPanel.SetActive(false);
    }

    // Update tampilan UI berdasarkan data dari CCTVManager
    public void UpdateStatus(List<CCTVCamera> allCameras)
    {
        for (int i = 0; i < statusDisplays.Count; i++)
        {
            if (i < allCameras.Count)
            {
                statusDisplays[i].UpdateDisplay(allCameras[i]);
            }
        }
    }
}

// Skrip kecil ini kita buat di file yang sama untuk referensi UI
[System.Serializable]
public class CCTV_Status_UI
{
    public TMP_Text roomNameText;
    public TMP_Text statusText;

    public void UpdateDisplay(CCTVCamera camera)
    {
        roomNameText.text = camera.roomName;
        if (camera.isOffline)
        {
            statusText.text = "OFFLINE";
            statusText.color = Color.red;
        }
        else
        {
            statusText.text = "ONLINE";
            statusText.color = Color.green;
        }
    }
}