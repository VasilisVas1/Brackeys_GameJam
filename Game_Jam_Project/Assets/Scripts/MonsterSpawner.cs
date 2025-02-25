using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform player;
    public float spawnRadius = 15f;
    public float spawnRate = 2f;
    public float waveDuration = 120f;
    public GameObject objectToDeactivate; // Assign in Inspector

    public bool isSpawning = false;
    public List<GameObject> spawnedMonsters = new List<GameObject>();

    // Narration components
    public AudioSource audioSource;
    public AudioClip narrationClip;
    public GameObject speechBubble;
    public TMP_Text subtitleText;
    public string subtitleTextContent;
    public RectTransform bubbleRectTransform;
    public bool hasPlayedNarration = false; // To ensure narration only plays once

    public void StartWave()
    {
        StopAllCoroutines(); // Ensure no previous coroutine is running
        isSpawning = true;
        hasPlayedNarration = false; // Reset for a new wave
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

            // Play narration only for the first spawned monster
            if (!hasPlayedNarration)
            {
                hasPlayedNarration = true;
                //StartCoroutine(PlayNarration());
                //test test test
                if (narrationCoroutine != null) // Stop any existing narration
            {
                StopCoroutine(narrationCoroutine);
            }

            narrationCoroutine = StartCoroutine(PlayNarration());
            }
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
    public Coroutine narrationCoroutine; // Store the coroutine


    private IEnumerator PlayNarration()
    {
        if (narrationClip != null && audioSource != null && speechBubble != null)
        {
            // Enable the speech bubble
            speechBubble.SetActive(true);

            // Set subtitle text
            subtitleText.text = subtitleTextContent;

            // Force UI to update its size
            LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleRectTransform);

            // Play audio
            audioSource.clip = narrationClip;
            audioSource.Play();

            // Wait for audio to finish
            yield return new WaitForSeconds(narrationClip.length);

            // Clear text and hide speech bubble
            subtitleText.text = "";
            speechBubble.SetActive(false);

            narrationCoroutine = null; // Clear reference after it finishes

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
