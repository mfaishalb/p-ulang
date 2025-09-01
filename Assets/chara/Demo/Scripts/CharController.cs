using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Actions))]
public class TPPPlayerController : MonoBehaviour
{
    [Header("Refs")]
    public Transform cameraTransform; // drag Main Camera di sini (kalau kosong akan auto pakai Camera.main)

    [Header("Movement")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 6.5f;
    public float rotationLerp = 12f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Actions actions;

    private Vector3 velocity;    // hanya untuk sumbu Y (gravity/jump)
    private bool isAiming;       // tahan RMB untuk aim
    private bool isGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        actions = GetComponent<Actions>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // ===== Ground check & gravity =====
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f; // biar nempel tanah

        // ===== Input WASD =====
        float x = Input.GetAxisRaw("Horizontal"); // A/D atau ←/→
        float z = Input.GetAxisRaw("Vertical");   // W/S atau ↑/↓
        Vector2 input = new Vector2(x, z);
        input = input.sqrMagnitude > 1f ? input.normalized : input;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool hasMove = input.sqrMagnitude > 0.0001f;

        // ===== Camera-relative move =====
        Vector3 moveDir = Vector3.zero;
        if (hasMove && cameraTransform != null)
        {
            Vector3 camFwd = cameraTransform.forward; camFwd.y = 0f; camFwd.Normalize();
            Vector3 camRight = cameraTransform.right; camRight.y = 0f; camRight.Normalize();
            moveDir = (camFwd * input.y + camRight * input.x).normalized;

            // rotate karakter ke arah gerak
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerp * Time.deltaTime);
        }

        float currentSpeed = (isRunning ? runSpeed : walkSpeed);
        Vector3 horizontalMove = moveDir * currentSpeed;

        // ===== Apply horizontal move =====
        controller.Move(horizontalMove * Time.deltaTime);

        // ===== Animator via Actions =====
        if (hasMove)
        {
            if (isRunning) actions.Run();
            else actions.Walk();
        }
        else
        {
            if (!isAiming) actions.Stay();
        }

        // ===== Jump =====
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            actions.Jump();
        }

        // ===== Gravity apply =====
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ===== Aiming (hold RMB) & Attack (LMB while aiming) =====
        if (Input.GetMouseButtonDown(1)) { isAiming = true; actions.Aiming(); }
        if (Input.GetMouseButtonUp(1)) { isAiming = false; if (!hasMove) actions.Stay(); }

        if (isAiming && Input.GetMouseButtonDown(0))
            actions.Attack();

        // ===== Crouch toggle (C) pakai Sitting() =====
        if (Input.GetKeyDown(KeyCode.C))
            actions.Sitting();

        // ===== Optional: debug keys =====
        if (Input.GetKeyDown(KeyCode.H)) actions.Damage();
        if (Input.GetKeyDown(KeyCode.K)) actions.Death();
    }
}
