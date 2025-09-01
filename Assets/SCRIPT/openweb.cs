using UnityEngine;

public class TriggerText : MonoBehaviour
{
    public Transform player;          // taruh player kamu
    public float triggerDistance = 3f; // jarak trigger
    public GameObject worldText;      // assign teks child di inspector
    public string url = "https://google.com"; // ganti dengan URL kamu

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= triggerDistance)
        {
            worldText.SetActive(true);   // tampilkan teks

            // cek input untuk buka website
            if (Input.GetKeyDown(KeyCode.E))
            {
                Application.OpenURL(url);
            }
        }
        else
        {
            worldText.SetActive(false);  // sembunyikan teks
        }
    }
}
