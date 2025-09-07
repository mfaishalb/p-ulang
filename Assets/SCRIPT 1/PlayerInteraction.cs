using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    Interactable currentInteractable;
    private Camera playerCamera; // cache kamera sekali

    void Start()
    {
        playerCamera = Camera.main; // pastikan kamera bertag MainCamera
    }

    void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
       
    }

    void CheckInteraction()
    {
        // Tentukan titik pusat untuk 'bola' pengecekan, misalnya sedikit di depan pemain
        Vector3 checkPosition = transform.position + (transform.forward * 1.0f); // 1m di depan pemain
        float checkRadius = 1.5f; // Radius 'bola' pengecekan

        // Dapatkan semua collider yang masuk ke dalam 'bola'
        Collider[] colliders = Physics.OverlapSphere(checkPosition, checkRadius);

        Interactable closestInteractable = null;
        float minDistance = float.MaxValue;

        // Cari objek interactable terdekat dari semua yang terdeteksi
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Interactable"))
            {
                Interactable interactable = col.GetComponent<Interactable>();
                if (interactable != null && interactable.enabled)
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }
        }

        // Jika kita menemukan objek interactable terdekat
        if (closestInteractable != null)
        {
            SetNewCurrentInteractable(closestInteractable);
        }
        else // Jika tidak ada objek interactable di dalam area
        {
            DisableCurrentInteractable();
        }
    }

    // Untuk debugging visual di Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 checkPosition = transform.position + (transform.forward * 1.0f);
        float checkRadius = 1.5f;
        Gizmos.DrawWireSphere(checkPosition, checkRadius);
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        if (currentInteractable != newInteractable)
        {
            if (currentInteractable != null) currentInteractable.DisableOutline();
            currentInteractable = newInteractable;
            currentInteractable.EnableOutline();
            HUDController.instance.EnableInteractionText(currentInteractable.message);
        }
    }

    void DisableCurrentInteractable()
    {
        // Pastikan kita memanggil fungsi untuk menonaktifkan teks di HUD
        if (HUDController.instance != null)
        {
            HUDController.instance.DisableInteractionText();
        }

        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
