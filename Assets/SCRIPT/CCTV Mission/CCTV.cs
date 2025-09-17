using UnityEngine;

public class CCTVCamera : MonoBehaviour
{
    [Header("Camera Info")]
    public string roomName;

    [Header("Visuals")]
    public GameObject cameraLight;
    public Color offlineColor = Color.red;
    public Color onlineColor = Color.green;

    private CCTV_RepairMission repairMission;
    private Mission_CCTV parentMission;

    [HideInInspector]
    public bool isOffline = false;

    void Awake()
    {
        repairMission = GetComponent<CCTV_RepairMission>();
        if (cameraLight != null) cameraLight.GetComponent<Renderer>().material.color = onlineColor;

        // Pastikan skrip perbaikan mati total di awal
        if (repairMission != null) repairMission.enabled = false;
    }

    public void GoOffline(Mission_CCTV missionController)
    {
        parentMission = missionController;
        isOffline = true;
        if (cameraLight != null) cameraLight.GetComponent<Renderer>().material.color = offlineColor;

        // Saat sabotase, kita HANYA mengaktifkan skripnya.
        // Skripnya akan menunggu player masuk ke zona trigger.
        if (repairMission != null)
        {
            repairMission.enabled = true;
            repairMission.StartMission(); // Panggil ini untuk reset state
        }
    }

    public void ReportRepairComplete()
    {
        isOffline = false;
        if (cameraLight != null) cameraLight.GetComponent<Renderer>().material.color = onlineColor;

        if (parentMission != null)
        {
            parentMission.OnCameraRepaired(this);
        }
    }
}