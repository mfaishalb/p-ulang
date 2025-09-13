using UnityEngine;

public class CCTVCamera : MonoBehaviour
{
    [Header("Camera Info")]
    public string roomName; // Isi di Inspector, contoh: "Ruang Medis"

    [Header("Components")]
    public GameObject cameraLight; // Opsional: Lampu merah/hijau di CCTV
    public Mission_HoldKey repairMission; // Misi 'Hold F' yang ada di CCTV ini

    [HideInInspector]
    public bool isOffline = false;

    // Referensi ke misi CCTV utama yang sedang berjalan
    private Mission_CCTV parentMission;

    void Awake()
    {
        // Pastikan misi perbaikan nonaktif di awal
        if (repairMission != null)
        {
            repairMission.enabled = false;
        }
    }

    // Dipanggil oleh CCTVManager saat sabotase dimulai
    public void GoOffline(Mission_CCTV missionController)
    {
        parentMission = missionController;
        isOffline = true;
        if (cameraLight != null) cameraLight.GetComponent<Renderer>().material.color = Color.red;

        // Aktifkan misi 'Hold F' agar bisa diperbaiki
        if (repairMission != null)
        {
            repairMission.enabled = true;
        }
    }

    // Dipanggil oleh Mission_HoldKey saat perbaikan selesai
    public void GoOnline()
    {
        isOffline = false;
        if (cameraLight != null) cameraLight.GetComponent<Renderer>().material.color = Color.green;

        // Nonaktifkan kembali misi 'Hold F'
        if (repairMission != null)
        {
            repairMission.enabled = false;
        }

        // Lapor ke misi utama bahwa 1 CCTV sudah beres!
        if (parentMission != null)
        {
            parentMission.OnCameraRepaired(this);
        }
    }
}