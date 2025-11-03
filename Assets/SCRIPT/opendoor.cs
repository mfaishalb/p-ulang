using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InteractableDoor : MonoBehaviour
{
    [Tooltip("Nama parameter BOOLEAN di Animator, contoh: 'IsOpen'")]
    public string parameterName = "IsOpen";

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Ini adalah fungsi yang akan kita panggil dari UnityEvent
    public void ToggleDoorState()
    {
        // 1. Dapatkan kondisi pintu saat ini (sedang terbuka atau tertutup)
        bool currentState = animator.GetBool(parameterName);

        // 2. Set Animator ke kondisi kebalikannya
        animator.SetBool(parameterName, !currentState);

        Debug.Log("Door state toggled to: " + !currentState);
    }
}