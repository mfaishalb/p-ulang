using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    // Setiap misi perlu tahu siapa 'pemiliknya'
    protected SabotageableObject owner;

    // Fungsi untuk inisialisasi, dipanggil oleh pemiliknya
    public virtual void Initialize(SabotageableObject ownerObject)
    {
        this.owner = ownerObject;
    }

    // Semua turunan WAJIB memiliki fungsi ini, tapi isinya bisa beda-beda
    public abstract void StartMission();
}