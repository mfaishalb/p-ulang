using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Actions))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    public Transform cameraTransform; // drag Main Camera kalau mau
    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    [Header("Movement")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 6.5f;
    public float rotationLerp = 12f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Actions actions;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isAiming;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (arsenal.Length > 0)
            SetArsenal(arsenal[0].name);
    }

    void Update()
    {
        // ===== Ground Check =====
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        // ===== Input =====
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(x, z);
        if (input.sqrMagnitude > 1f) input.Normalize();

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool hasMove = input.sqrMagnitude > 0.001f;

        // ===== Camera Relative Movement =====
        Vector3 moveDir = Vector3.zero;
        if (hasMove && cameraTransform != null)
        {
            Vector3 camFwd = cameraTransform.forward; camFwd.y = 0f; camFwd.Normalize();
            Vector3 camRight = cameraTransform.right; camRight.y = 0f; camRight.Normalize();
            moveDir = (camFwd * input.y + camRight * input.x).normalized;

            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerp * Time.deltaTime);
        }

        // ===== Apply Horizontal Movement =====
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        // ===== Animator Movement =====
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

        // ===== Apply Gravity =====
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ===== Aiming & Attack =====
        if (Input.GetMouseButtonDown(1)) { isAiming = true; actions.Aiming(); }
        if (Input.GetMouseButtonUp(1)) { isAiming = false; if (!hasMove) actions.Stay(); }

        if (isAiming && Input.GetMouseButtonDown(0))
            actions.Attack();

        // ===== Crouch =====
        if (Input.GetKeyDown(KeyCode.C))
            actions.Sitting();

        // ===== Debug Keys =====
        if (Input.GetKeyDown(KeyCode.H)) actions.Damage();
        if (Input.GetKeyDown(KeyCode.K)) actions.Death();
    }

    // ===== Arsenal Logic (dari script lama) =====
    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);

                if (hand.rightGun != null)
                {
                    GameObject newRightGun = Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
}
