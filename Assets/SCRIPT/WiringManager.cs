using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public string nodeColor = "Red";          // samakan antar pasangan, contoh: "Red", "Blue", "Green"
    public WireTaskManager manager;           // drag & drop reference ke manager di Inspector

    RectTransform rect;

    void Awake() => rect = GetComponent<RectTransform>();

    public void OnPointerDown(PointerEventData e)
    {
        manager.BeginDrag(this, e.position);
    }

    public void OnDrag(PointerEventData e)
    {
        manager.DragTo(e.position);
    }

    public void OnPointerUp(PointerEventData e)
    {
        manager.EndDrag(e.position);
    }
}
