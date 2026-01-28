using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabA;
    [SerializeField] private GameObject prefabB;
    [SerializeField] private float spawnInterval = 1f;

    private Coroutine spawnRoutine;

    private void OnEnable()
    {
        if (spawnRoutine == null)
        {
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnOne();
            yield return new WaitForSeconds(Mathf.Max(0.01f, spawnInterval));
        }
    }

    private void SpawnOne()
    {
        if (prefabA == null && prefabB == null)
        {
            return;
        }

        GameObject prefab = Random.value < 0.5f ? prefabA : prefabB;
        if (prefab == null)
        {
            prefab = prefabA != null ? prefabA : prefabB;
        }

        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
