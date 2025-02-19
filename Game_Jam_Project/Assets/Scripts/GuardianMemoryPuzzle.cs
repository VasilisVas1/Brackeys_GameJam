using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardianMemoryPuzzle : MonoBehaviour
{
    public List<GameObject> mushrooms; // Assign in Inspector
    private List<int> sequence = new List<int>();
    private List<int> playerInput = new List<int>();
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    public float delayBetweenLights = 1f;
    private bool playerTurn = false;
    private bool playerInRange = false;
    private bool puzzleSolved = false; // Prevent replaying
    private bool puzzleInProgress = false; // Prevent restarting while playing
    private bool failedAttempt = false; // Track if player failed

    public GameObject rewardObject; // Assign the missing piece prefab
    public GameObject uiMessage; // Assign a UI Text or Panel ("Press E to start")
    public GameObject retryMessage; // Assign a UI Text ("Press R to retry")

    void Start()
    {
       // uiMessage.SetActive(false); 
       // retryMessage.SetActive(false);

        // Store original colors of mushrooms
        foreach (GameObject mushroom in mushrooms)
        {
            Renderer rend = mushroom.GetComponent<Renderer>();
            originalColors[mushroom] = rend.material.color;
        }
    }

    void Update()
    {
        if (playerInRange && !puzzleSolved && !puzzleInProgress && !failedAttempt && Input.GetKeyDown(KeyCode.E))
        {
            uiMessage.SetActive(false);
            StartPuzzle();
        }

        if (failedAttempt && Input.GetKeyDown(KeyCode.R))
        {
            retryMessage.SetActive(false);
            failedAttempt = false;
            StartPuzzle();
        }
    }

    void StartPuzzle()
    {
        sequence.Clear();
        playerInput.Clear();
        puzzleInProgress = true; // Lock puzzle from being restarted
        playerTurn = false; // Block player from interacting until sequence ends
        failedAttempt = false;
        GenerateSequence();
        StartCoroutine(PlaySequence());
    }

    void GenerateSequence()
    {
        List<int> availableIndices = new List<int>();

        // Add all mushroom indices to available list
        for (int i = 0; i < mushrooms.Count; i++)
        {
            availableIndices.Add(i);
        }

        while (availableIndices.Count > 0)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            sequence.Add(availableIndices[randomIndex]);
            availableIndices.RemoveAt(randomIndex);
        }
    }

    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(1f);

        foreach (int index in sequence)
        {
            GameObject mushroom = mushrooms[index];
            LightUpMushroom(mushroom);
            yield return new WaitForSeconds(delayBetweenLights);
            ResetMushroom(mushroom);
            yield return new WaitForSeconds(0.5f);
        }

        playerTurn = true; // Now player can interact
    }

    public void PlayerSelectsMushroom(int mushroomIndex)
    {
        if (!playerTurn || puzzleSolved || failedAttempt) return; // Prevent input before sequence ends

        GameObject selectedMushroom = mushrooms[mushroomIndex];
        playerInput.Add(mushroomIndex);
        LightUpMushroom(selectedMushroom);
        StartCoroutine(ResetAfterDelay(selectedMushroom, 0.5f));

        if (playerInput.Count > sequence.Count || playerInput[playerInput.Count - 1] != sequence[playerInput.Count - 1])
        {
            // Wrong choice, allow retry instead of auto-restart
            Debug.Log("Wrong choice! Press R to retry.");
            failedAttempt = true;
            retryMessage.SetActive(true);
            playerTurn = false; // Block further inputs
        }
        else if (playerInput.Count == sequence.Count)
        {
            CheckPlayerInput();
        }
    }

public AudioSource successAudio; // Assign in Inspector

void CheckPlayerInput()
{
    for (int i = 0; i < sequence.Count; i++)
    {
        if (playerInput[i] != sequence[i])
        {
            Debug.Log("Wrong sequence! Press R to retry.");
            failedAttempt = true;
            retryMessage.SetActive(true);
            playerTurn = false; // Block further inputs
            return;
        }
    }

    Debug.Log("Correct! The guardian grants you the missing piece.");
    puzzleSolved = true; // Prevent replaying the puzzle
    puzzleInProgress = false;

    // Play success sound before granting reward
    StartCoroutine(PlaySuccessSoundAndGrantReward());
}

IEnumerator PlaySuccessSoundAndGrantReward()
{
    if (successAudio != null)
    {
        successAudio.Play();
        yield return new WaitForSeconds(successAudio.clip.length); // Wait until the audio finishes
    }

    rewardObject.SetActive(true); // Activate reward after sound ends
}


    public AudioSource mushroomSound; // Assign a sound effect in the Inspector

public ParticleSystem poofEffect; // Assign in Inspector

void LightUpMushroom(GameObject mushroom)
{
    Renderer rend = mushroom.GetComponent<Renderer>();
    rend.material.color = Color.yellow;

    // Play mushroom sound
    if (mushroomSound != null)
    {
        mushroomSound.Play();
    }

    // Spawn poof effect
    if (poofEffect != null)
    {
        ParticleSystem poof = Instantiate(poofEffect, mushroom.transform.position, Quaternion.identity);
        poof.Play();
        Destroy(poof.gameObject, 1f); // Destroy after effect finishes
    }
}



    void ResetMushroom(GameObject mushroom)
    {
        Renderer rend = mushroom.GetComponent<Renderer>();
        rend.material.color = originalColors[mushroom];
    }

    IEnumerator ResetAfterDelay(GameObject mushroom, float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetMushroom(mushroom);
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player") && !puzzleSolved)
    {
        playerInRange = true;
        if (failedAttempt)
        {
            retryMessage.SetActive(true); // Show retry message again if failed
        }
        else
        {
            uiMessage.SetActive(!puzzleInProgress);
        }
    }
}


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiMessage.SetActive(false);
            retryMessage.SetActive(false);
        }
    }
}
