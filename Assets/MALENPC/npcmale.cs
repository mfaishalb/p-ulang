using UnityEngine;
using System.Collections;

public class SimpleNPC : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 2f;
    public float moveRadius = 5f;

    private Vector3 startPoint;
    private Vector3 targetPoint;
    private bool isWalking = false;

    void Start()
    {
        startPoint = transform.position;
        StartCoroutine(NPCRoutine());
    }

    void Update()
    {
        if (isWalking)
        {
            // arahkan ke target
            Vector3 dir = (targetPoint - transform.position).normalized;
            if (dir != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // jalan ke target
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, walkSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint) < 0.2f)
            {
                // selesai jalan → idle
                isWalking = false;
                animator.SetBool("isWalking", false);
                StartCoroutine(NPCRoutine());
            }
        }
    }

    IEnumerator NPCRoutine()
    {
        if (isWalking) yield break; // kalau lagi jalan, jangan tumpuk aksi baru

        // pilih idle atau jalan
        int action = Random.Range(0, 2);

        if (action == 0) // Idle
        {
            animator.SetBool("isWalking", false);
            yield return new WaitForSeconds(Random.Range(2f, 5f)); // idle sebentar
        }
        else if (action == 1) // Jalan
        {
            Vector2 randomCircle = Random.insideUnitCircle * moveRadius;
            targetPoint = startPoint + new Vector3(randomCircle.x, 0, randomCircle.y);

            isWalking = true;
            animator.SetBool("isWalking", true);
            yield break; // stop coroutine, biar jalan di Update()
        }

        StartCoroutine(NPCRoutine());
    }
}
