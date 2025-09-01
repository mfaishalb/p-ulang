using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SlidingDoor : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 leftOpenOffset = new Vector3(-1.5f, 0, 0);
    public Vector3 rightOpenOffset = new Vector3(1.5f, 0, 0);

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;

    private bool isOpen = false;
    public float speed = 2f;

    void Start()
    {
        leftClosedPos = leftDoor.localPosition;
        rightClosedPos = rightDoor.localPosition;
    }

    void Update()
    {
        

        // Transisi posisi
        Vector3 targetLeft = isOpen ? leftClosedPos + leftOpenOffset : leftClosedPos;
        Vector3 targetRight = isOpen ? rightClosedPos + rightOpenOffset : rightClosedPos;

        leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, targetLeft, Time.deltaTime * speed);
        rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, targetRight, Time.deltaTime * speed);
    }
    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

}


