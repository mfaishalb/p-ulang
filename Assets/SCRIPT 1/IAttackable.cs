using UnityEngine;

public interface IAttackable
{
    bool IsFixed { get; } // status sudah di-fix atau belum
    void TriggerAttack(); // dipanggil ketika objek terpilih kena serangan
    void Fix(); // dipanggil ketika player berhasil menyelesaikan task
}
