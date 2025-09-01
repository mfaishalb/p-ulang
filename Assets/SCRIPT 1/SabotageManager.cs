using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SabotageManager : MonoBehaviour
{
    public List<SabotageObject> sabotageObjects;
    public float minInterval = 20f;
    public float maxInterval = 30f;

    private void Start()
    {
        StartCoroutine(SabotageRoutine());
    }

    private IEnumerator SabotageRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // kalau masih ada object yang belum difix → skip
            if (sabotageObjects.Exists(o => !o.isFixed))
                continue;

            // pilih random object yang ready
            List<SabotageObject> candidates = sabotageObjects.FindAll(o => o.isFixed);
            if (candidates.Count > 0)
            {
                SabotageObject chosen = candidates[Random.Range(0, candidates.Count)];
                chosen.TriggerAttack();
                Debug.Log("Sabotage: " + chosen.name);
            }
        }
    }
}
