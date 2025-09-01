using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private bool showCursor = false; // atur per scene di inspector

    void Start()
    {
        if (showCursor)
        {
            // Mode untuk UI/menu
            Cursor.lockState = CursorLockMode.None; // bebas bergerak
            Cursor.visible = true;                  // tampil
        }
        else
        {
            // Mode untuk FPP/gameplay
            Cursor.lockState = CursorLockMode.Locked; // terkunci di tengah
            Cursor.visible = false;                   // disembunyikan
        }
    }
}
