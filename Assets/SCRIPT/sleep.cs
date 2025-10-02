using System.Collections;
using UnityEngine;

public class Mission_Sleep : Mission
{
    // Kamu bisa tambahkan referensi ke panel hitam untuk fade effect
    // public Image fadePanel; 

    public override void StartMission()
    {
        Debug.Log("Player sedang tidur...");

        // Langsung panggil fungsi untuk memulihkan energi
        PlayerEnergy.instance.RestoreEnergy();

        // Opsional: Tambahkan efek visual tidur, misal layar jadi hitam sejenak
        // StartCoroutine(SleepSequence());
    }

    /*
    // Contoh coroutine untuk efek tidur
    private IEnumerator SleepSequence()
    {
        // fadePanel.gameObject.SetActive(true);
        // yield return new WaitForSeconds(2f); // Tunggu 2 detik
        // fadePanel.gameObject.SetActive(false);
    }
    */
}