using UnityEngine;
using UnityEngine.SceneManagement;

public class SabotageObject : Interactable
{
    public bool isFixed = true;
    public GameObject warningSign;
    public float sabotageDuration = 10f;

    private float timer;

    public void TriggerAttack()
    {
        isFixed = false;
        warningSign.SetActive(true);
        timer = sabotageDuration;
    }

    public void Fix()
    {
        isFixed = true;
        warningSign.SetActive(false);
    }

    void Update()
    {
        if (!isFixed)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Debug.Log("Game Over");
                
            }
        }
    }

    public override void Interact() // override Interactable
    {
        if (!isFixed)
        {
            base.Interact(); // tetap jalanin UnityEvent → misal buka puzzle atau hold task
        }
    }
    public void OpenWireTask()
    {
        GameStateManager.Instance.lastSabotageTarget = gameObject.name;
        SceneManager.LoadScene("wiretask");
    }
}
