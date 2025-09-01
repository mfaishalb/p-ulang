using System.Collections;
using UnityEngine;

public class HoldScreen : MonoBehaviour
{
    [Header("UI Hold Screen")]
    [SerializeField] CanvasGroup holdCanvas;   // CanvasGroup dari layar info
    [SerializeField] GameObject hudRoot;        // Root HUD utama (boleh null kalau nggak ada)
    [SerializeField] bool fadeOnDismiss = true;
    [SerializeField] float fadeDuration = 0.25f;

    [Header("Apa yang dimatikan saat hold")]
    [SerializeField] GameObject[] objectsToDisable; // mis. Player, CameraRig, dll (root GO)
    [SerializeField] bool pauseTimeWhileHolding = false; // set true kalau mau freeze game

    [Header("Tampilkan sekali saja per sesi?")]
    [SerializeField] bool showOnlyOncePerRun = false;
    static bool alreadyShownThisRun = false;

    bool dismissed;

    void Awake()
    {
        // Kalau ingin cuma tampil sekali per sesi game
        if (showOnlyOncePerRun && alreadyShownThisRun)
        {
            SkipImmediately();
            return;
        }

        // Pastikan hold canvas aktif penuh dan blok input
        if (holdCanvas != null)
        {
            holdCanvas.gameObject.SetActive(true);
            holdCanvas.alpha = 1f;
            holdCanvas.interactable = true;
            holdCanvas.blocksRaycasts = true;
        }

        // Matikan HUD dan kontrol
        if (hudRoot != null) hudRoot.SetActive(false);
        foreach (var go in objectsToDisable)
            if (go) go.SetActive(false);

        if (pauseTimeWhileHolding) Time.timeScale = 0f;
    }

    void Update()
    {
        if (dismissed) return;

        // Deteksi input "apa saja": keyboard, mouse, atau touch
        if (Input.anyKeyDown) { TryDismiss(); return; }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) { TryDismiss(); return; }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { TryDismiss(); return; }

        // Opsional: kalau pakai New Input System, blok ini ikut nangkep
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame) { TryDismiss(); return; }
        foreach (var pad in UnityEngine.InputSystem.Gamepad.all)
            if (pad != null && pad.allControls.Exists(c => c is UnityEngine.InputSystem.Controls.ButtonControl b && b.wasPressedThisFrame))
            { TryDismiss(); return; }
#endif
    }

    void TryDismiss()
    {
        if (dismissed) return;
        dismissed = true;
        alreadyShownThisRun = true;
        StartCoroutine(DismissRoutine());
    }

    IEnumerator DismissRoutine()
    {
        if (pauseTimeWhileHolding) Time.timeScale = 1f;

        // Balikin kontrol dan HUD
        foreach (var go in objectsToDisable)
            if (go) go.SetActive(true);
        if (hudRoot != null) hudRoot.SetActive(true);

        // Lepas blok input UI
        if (holdCanvas != null)
        {
            holdCanvas.interactable = false;
            holdCanvas.blocksRaycasts = false;

            if (fadeOnDismiss && fadeDuration > 0f)
            {
                float t = 0f;
                float start = holdCanvas.alpha;
                // pakai unscaled time supaya tetap jalan kalau tadi pause
                while (t < fadeDuration)
                {
                    t += Time.unscaledDeltaTime;
                    holdCanvas.alpha = Mathf.Lerp(start, 0f, t / fadeDuration);
                    yield return null;
                }
                holdCanvas.alpha = 0f;
            }

            holdCanvas.gameObject.SetActive(false);
        }
    }

    void SkipImmediately()
    {
        // Dipanggil kalau kita set "show once" dan sudah pernah tampil
        if (pauseTimeWhileHolding) Time.timeScale = 1f;

        foreach (var go in objectsToDisable)
            if (go) go.SetActive(true);
        if (hudRoot != null) hudRoot.SetActive(true);

        if (holdCanvas != null)
        {
            holdCanvas.alpha = 0f;
            holdCanvas.interactable = false;
            holdCanvas.blocksRaycasts = false;
            holdCanvas.gameObject.SetActive(false);
        }

        dismissed = true;
    }
}
