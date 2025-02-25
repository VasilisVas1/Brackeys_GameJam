using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public Transform respawnPoint;  // Set this in the inspector
    public GameObject player;
    public MonsterSpawner monsterSpawner; // Reference to the spawner
    public GameObject wand;

    public Transform wandOriginalPosition; // Set original position in Inspector
    public GameObject wallToDisable; // The wall that disappears when picking up the wand

    private WandShooting wandShooting;
    private PickUpWand pickUpWand;
    private Collider wandCollider;

    public AudioSource playerHit;


    void Start()
{
    if (wand != null)
    {
        wandShooting = wand.GetComponent<WandShooting>();
        pickUpWand = wand.GetComponent<PickUpWand>();
        wandCollider = wand.GetComponent<Collider>();
    }
}


    public void RespawnPlayer()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
{
    // Stop narration if it's playing
    if (monsterSpawner != null && monsterSpawner.narrationCoroutine != null)
    {
                    playerHit.Play();

        monsterSpawner.StopCoroutine(monsterSpawner.narrationCoroutine);
        monsterSpawner.narrationCoroutine = null;
        monsterSpawner.audioSource.Stop();

        if (monsterSpawner.subtitleText != null)
        {
            monsterSpawner.subtitleText.text = "";
        }

        if (monsterSpawner.speechBubble != null)
        {
            monsterSpawner.speechBubble.SetActive(false);
        }
    }

    // Destroy all remaining monsters
    DestroyAllMonsters();
    
    // Reset spawner
    if (monsterSpawner != null)
    {
        monsterSpawner.StopAllCoroutines(); // Stop any ongoing spawns
        monsterSpawner.isSpawning = false;
        monsterSpawner.hasPlayedNarration = false; // Allow narration again
        monsterSpawner.spawnedMonsters.Clear();
        monsterSpawner.objectToDeactivate?.SetActive(true);
    }

    // Reset Wand
    ResetWand();

    // Reset player position
    player.transform.position = respawnPoint.position;

    // Resume game
    Time.timeScale = 1;

    yield return null; // Ensure coroutine completes properly
}


    private void DestroyAllMonsters()
    {
        foreach (GameObject monster in monsterSpawner.spawnedMonsters)
        {
            if (monster != null)
            {
                Destroy(monster);
            }
        }
    }

    private void ResetWand()
    {
        if (wand != null)
        {
            wand.transform.SetParent(null); // Detach from player hand
            wand.transform.position = wandOriginalPosition.position;
            wand.transform.rotation = wandOriginalPosition.rotation;
            wandCollider.enabled = true; // Enable collider so it can be picked up again

            if (wandShooting != null)
            {
                wandShooting.canShoot = false; // Disable shooting
                pickUpWand.isPlayerInRange=false;
            }

            if (pickUpWand != null)
            {
                pickUpWand.enabled = true; // Allow pickup interaction again
            }
        }

        // Re-enable the wall (if applicable)
        if (wallToDisable != null)
        {
            wallToDisable.SetActive(true);
        }
    }
}
