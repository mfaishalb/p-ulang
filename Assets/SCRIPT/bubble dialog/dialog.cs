using UnityEngine;

// Ini akan membuat menu baru di Unity untuk membuat aset dialog
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/New Dialogue")]
public class DialogueData : ScriptableObject
{
    public string characterName;
    public Sprite characterPortrait; // Gambar/avatar karakter

    [TextArea(3, 10)] // Membuat kolom teks lebih besar di Inspector
    public string dialogueText;
}