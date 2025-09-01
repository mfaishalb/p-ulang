using UnityEngine;

public class SceneRestore : MonoBehaviour
{
    private void Start()
    {
        // Pastikan ada instance GameStateManager
        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("[Restore] GameStateManager belum ada di scene ini!");
            return;
        }

        // Ambil nama target sabotage terakhir
        if (!string.IsNullOrEmpty(GameStateManager.Instance.lastSabotageTarget))
        {
            Debug.Log($"[Restore] Cek object {GameStateManager.Instance.lastSabotageTarget}");

            // Cari object dengan nama yang sama di scene
            var obj = GameObject.Find(GameStateManager.Instance.lastSabotageTarget);
            if (obj != null)
            {
                var sabotage = obj.GetComponent<SabotageObject>();
                if (sabotage != null)
                {
                    sabotage.Fix();
                    Debug.Log($"[Restore] {obj.name} berhasil diperbaiki!");
                }
                else
                {
                    Debug.LogWarning($"[Restore] {obj.name} tidak punya component SabotageObject!");
                }
            }
            else
            {
                Debug.LogWarning($"[Restore] Object {GameStateManager.Instance.lastSabotageTarget} tidak ditemukan di scene!");
            }

            // Reset state biar ga ke-restore lagi
            GameStateManager.Instance.lastSabotageTarget = null;
        }
    }
}
