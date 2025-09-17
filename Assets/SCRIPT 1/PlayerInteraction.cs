using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    Interactable currentInteractable;
    private Camera playerCamera;

    // BARIS BARU: Variabel publik yang bisa diakses dari skrip lain
    public static Interactable currentInteractablePublic;

    void Start()
    {
        playerCamera = Camera.main;
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
        // Logika OverlapSphere-mu sudah bagus, tidak perlu diubah
        Vector3 checkPosition = transform.position + (transform.forward * 1.0f);
        float checkRadius = 3f;

        Collider[] colliders = Physics.OverlapSphere(checkPosition, checkRadius);

        Interactable closestInteractable = null;
        float minDistance = float.MaxValue;

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

        if (closestInteractable != null)
        {
            SetNewCurrentInteractable(closestInteractable);
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 checkPosition = transform.position + (transform.forward * 1.0f);
        float checkRadius = 3f;
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

            // BARIS BARU: Update variabel publik saat ada interaksi baru
            currentInteractablePublic = currentInteractable;
        }
    }

    void DisableCurrentInteractable()
    {
        if (HUDController.instance != null)
        {
            HUDController.instance.DisableInteractionText();
        }

        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;

            // BARIS BARU: Update variabel publik saat tidak ada interaksi
            currentInteractablePublic = null;
        }
    }
}