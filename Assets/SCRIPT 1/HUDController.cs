using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [SerializeField] GameObject interactionUIGroup; // INI UNTUK PARENT-NYA
    private TMP_Text interactionText; // Ini untuk komponen text di dalamnya
    private void Awake()
    {
        instance = this;

        if (interactionUIGroup == null)
        {
            Debug.LogError("InteractionUIGroup belum di-assign di HUDController!");
            return;
        }

        // Cari komponen TMP_Text di dalam anak-anak 'interactionUIGroup'
        interactionText = interactionUIGroup.GetComponentInChildren<TMP_Text>();

        // Pastikan UI-nya nonaktif di awal game
        interactionUIGroup.SetActive(false);
    }

    public void EnableInteractionText(string text)
    {
        // Logika mengubah teks tetap sama
        interactionText.text = text ;

        // PERBAIKAN: Tampilkan seluruh grup UI, bukan hanya teksnya
        interactionUIGroup.SetActive(true);
    }

    public void DisableInteractionText()
    {
        // PERBAIKAN: Sembunyikan seluruh grup UI
        interactionUIGroup.SetActive(false);
    }
}