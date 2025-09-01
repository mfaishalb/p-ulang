
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoIntro : MonoBehaviour
{
    public CanvasGroup canvasGroup;  // Drag CanvasGroup dari LogoCanvas
    public float fadeDuration = 1f;  // Waktu fade in/out
    public float logoDisplayTime = 2f; // Berapa lama logo tampil sebelum fade out

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // Awal alpha 0 (invisible)
        canvasGroup.alpha = 0f;

        // Fade IN
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Tunggu logo tampil
        yield return new WaitForSeconds(logoDisplayTime);

        // Fade OUT
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - (t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        // Ganti ke StartScene
        SceneManager.LoadScene("logo");
    }
}

