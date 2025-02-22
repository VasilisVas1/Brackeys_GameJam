using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform player;
    public float spawnRadius = 15f;
    public float spawnRate = 2f;
    public float waveDuration = 120f;
    public GameObject objectToDeactivate; // Assign in Inspector

    private bool isSpawning = false;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    public void StartWave()
    {
        StopAllCoroutines(); // Ensure no previous coroutine is running
        isSpawning = true;
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        float endTime = Time.time + waveDuration;

        while (Time.time < endTime - spawnRate) // Prevent timing issues
        {
            SpawnMonster();
            yield return new WaitForSeconds(spawnRate);
        }

        isSpawning = false;
        StartCoroutine(CheckForRemainingMonsters());
    }

    private void SpawnMonster()
    {
        if (monsterPrefab == null || player == null) return;

        Vector3 randomSpawnPosition = player.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            10f,  // Start above ground to raycast downward
            Random.Range(-spawnRadius, spawnRadius)
        );

        // Raycast downward to find ground level
        if (Physics.Raycast(randomSpawnPosition, Vector3.down, out RaycastHit hit, 20f))
        {
            GameObject monster = Instantiate(monsterPrefab, hit.point, Quaternion.identity);
            spawnedMonsters.Add(monster);
            monster.AddComponent<MonsterTracker>().spawner = this; // Attach tracker
        }
        else
        {
            Debug.LogWarning("Monster spawn position is not on the ground.");
        }
    }

    public void RemoveMonster(GameObject monster)
    {
        spawnedMonsters.Remove(monster);
        if (!isSpawning && spawnedMonsters.Count == 0 && objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
    }

    private IEnumerator CheckForRemainingMonsters()
    {
        while (spawnedMonsters.Count > 0)
        {
            yield return new WaitForSeconds(1f);
        }

        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
    }
}

public class MonsterTracker : MonoBehaviour
{
    public MonsterSpawner spawner;
    
    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.RemoveMonster(gameObject);
        }
    }
}
