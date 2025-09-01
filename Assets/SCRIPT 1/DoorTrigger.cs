using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTrigger : MonoBehaviour
{
    public SlidingDoor slidingDoor; // Reference ke script SlidingDoor
    public GameObject promptUI;     // UI "Press E to Open"

    private bool isPlayerInRange = false;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            slidingDoor.ToggleDoor(); // Fungsi untuk buka/tutup pintu
            promptUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}

