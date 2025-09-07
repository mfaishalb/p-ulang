using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireTaskManager : MonoBehaviour
{
    [Header("UI/Canvas")]
    public Canvas uiCanvas;                  // isi dengan Canvas HUD kamu
    public Camera uiCamera;                  // biasanya sama dengan Main Camera
    public GraphicRaycaster raycaster;       // component di Canvas (drag ke sini)

    [Header("Line")]
    public Material wireMaterial;            // pakai Material Shader Sprites/Default
    public float wireWidth = 0.05f;

    LineRenderer currentLine;
    WireNode startNode;

    // ==================== DRAG LIFECYCLE ====================

    public void BeginDrag(WireNode node, Vector2 screenPos)
    {
        startNode = node;

        // buat GO baru untuk line
        var go = new GameObject("WireLine_" + node.nodeColor);
        currentLine = go.AddComponent<LineRenderer>();

        // konfigurasi agar tampil di UI
        currentLine.material = wireMaterial;
        currentLine.sortingLayerName = "UI";   // pastikan ada Sorting Layer "UI" (default ada)
        currentLine.sortingOrder = 10;
        currentLine.alignment = LineAlignment.View;
        currentLine.useWorldSpace = true;
        currentLine.numCornerVertices = 8;
        currentLine.numCapVertices = 8;
        currentLine.positionCount = 2;
        currentLine.startWidth = wireWidth;
        currentLine.endWidth = wireWidth;

        // warna kabel mengikuti warna node (ambil dari Image kalau ada)
        var img = node.GetComponent<Image>();
        if (img) currentLine.startColor = currentLine.endColor = img.color;

        Vector3 startWorld = ScreenToWorldOnCanvas(screenPos);
        currentLine.SetPosition(0, startWorld);
        currentLine.SetPosition(1, startWorld);
    }

    public void DragTo(Vector2 screenPos)
    {
        if (currentLine == null) return;
        currentLine.SetPosition(1, ScreenToWorldOnCanvas(screenPos));
    }

    public void EndDrag(Vector2 screenPos)
    {
        if (currentLine == null) return;

        // raycast UI untuk mencari node yang dilepas
        var results = RaycastUI(screenPos);
        WireNode target = null;
        foreach (var r in results)
        {
            target = r.gameObject.GetComponent<WireNode>();
            if (target != null && target != startNode) break;
            target = null;
        }

        if (target != null && target.nodeColor == startNode.nodeColor)
        {
            // cocok: snap ujung garis ke posisi target
            Vector3 endWorld = ScreenToWorldOnCanvas(screenPos);
            currentLine.SetPosition(1, endWorld);
            // opsional: matikan interaksi pada dua node yang sudah terhubung
            SetNodeInteractable(startNode, false);
            SetNodeInteractable(target, false);
            // reset state, tapi biarkan line menetap
            currentLine = null;
            startNode = null;
        }
        else
        {
            // salah: hapus garis
            Destroy(currentLine.gameObject);
            currentLine = null;
            startNode = null;
        }
    }

    // ==================== HELPERS ====================

    Vector3 ScreenToWorldOnCanvas(Vector2 screenPos)
    {
        // konversi posisi mouse ke world point di kamera UI
        RectTransform canvasRect = uiCanvas.transform as RectTransform;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, screenPos, uiCamera, out var world))
        {
            // kecilkan Z supaya pasti di depan camera UI
            world.z = 0f;
            return world;
        }
        // fallback
        Vector3 wp = uiCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0.1f));
        wp.z = 0f;
        return wp;
    }

    List<RaycastResult> RaycastUI(Vector2 screenPos)
    {
        var ped = new PointerEventData(EventSystem.current) { position = screenPos };
        var results = new List<RaycastResult>();
        raycaster.Raycast(ped, results);
        return results;
    }

    void SetNodeInteractable(WireNode node, bool on)
    {
        var cg = node.GetComponent<CanvasGroup>();
        if (!cg) cg = node.gameObject.AddComponent<CanvasGroup>();
        cg.interactable = on;
        cg.blocksRaycasts = on;
        cg.alpha = on ? 1f : 0.6f;
    }
}
