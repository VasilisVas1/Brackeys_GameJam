using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;  
    public Transform player;          
    public float spawnRadius = 15f;   
    public float spawnRate = 2f;      
    public float waveDuration = 120f; 

    private bool isSpawning = false;

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
        Instantiate(monsterPrefab, hit.point, Quaternion.identity);
    }
    else
    {
        Debug.LogWarning("Monster spawn position is not on the ground.");
    }
}

}
