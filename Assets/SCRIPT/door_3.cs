using UnityEngine;

public class DoorAutoOpen : MonoBehaviour
{
    public Animator animator;
    public string parameterName = "character_nearby";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(parameterName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(parameterName, false);
        }
    }
}
