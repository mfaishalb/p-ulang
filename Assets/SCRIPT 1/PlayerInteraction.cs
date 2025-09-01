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
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            IPickable pickable = currentInteractable.GetComponent<IPickable>();
            if (pickable != null)
            {
                ItemPickable itemPickable = currentInteractable.GetComponent<ItemPickable>();
                if (itemPickable != null)
                {
                    PlayerInventory playerInventory = GetComponent<PlayerInventory>();
                    playerInventory.AddItem(itemPickable.itemScriptableObject.item_type);
                    pickable.PickItem();
                }
            }
            else
            {
                currentInteractable.Interact();
            }
        }
    }

    void CheckInteraction()
    {
        if (playerCamera == null) return;

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * playerReach, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerReach))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (newInteractable != null && newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
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
        HUDController.instance.DisableInteractionText();
        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
