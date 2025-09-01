using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float idleScale = 1f;         // Skala normal
    public float pulseAmount = 0.05f;    // Seberapa besar pulse (0.05 = +5%)
    public float pulseSpeed = 2f;        // Kecepatan pulse
    public float pressedScale = 0.9f;    // Skala saat tombol ditekan

    private RectTransform rectTransform;
    private bool isPressed;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isPressed) return; // Jangan pulse kalau tombol sedang ditekan

        float scale = idleScale + Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseAmount;
        rectTransform.localScale = Vector3.one * scale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        rectTransform.localScale = Vector3.one * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
