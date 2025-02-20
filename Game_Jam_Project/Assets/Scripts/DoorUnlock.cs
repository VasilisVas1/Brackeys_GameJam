using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoorUnlock : MonoBehaviour
{
    public GameObject uiMessage;
    public Transform whiteRockSlot, blueRockSlot, redRockSlot;
    public Transform leftDoor, rightDoor;
    public float doorOpenDistance = 3f;
    public float rockMoveSpeed = 10f; // Increased for faster movement
    public float doorOpenSpeed = 5f; // Increased for faster door opening

    public GameObject whiteRock, blueRock, redRock;

    private bool playerInRange = false;
    private bool isDoorUnlocked = false;
    private Transform player;

    public AudioSource lockSound;
    public AudioSource unlockSound;

    void Start()
    {
        uiMessage.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryToOpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform; // Get player position
            playerInRange = true;
            uiMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiMessage.SetActive(false);
        }
    }

    private void TryToOpenDoor()
    {
        if (RockCollectionManager.hasWhiteRock && RockCollectionManager.hasBlueRock && RockCollectionManager.hasRedRock)
        {
            DisableFloatingScript(whiteRock);
            DisableFloatingScript(blueRock);
            DisableFloatingScript(redRock);

            whiteRock.SetActive(true);
            blueRock.SetActive(true);
            redRock.SetActive(true);
            StartCoroutine(UnlockDoor());
        }
        else
        {
            lockSound.Play();
            Debug.Log("Door is locked! Collect all three rocks.");
        }
    }

    private void DisableFloatingScript(GameObject rock)
    {
        if (rock != null)
        {
            FloatingRock floatingScript = rock.GetComponent<FloatingRock>();
            if (floatingScript != null)
            {
                floatingScript.enabled = false; // Disable floating behavior
            }
        }
    }

    private IEnumerator UnlockDoor()
    {
        isDoorUnlocked = true;
        uiMessage.SetActive(false);
        unlockSound.Play();

        if (whiteRockSlot != null) yield return StartCoroutine(FloatRockToSlot(whiteRock, whiteRockSlot.position));
        if (blueRockSlot != null) yield return StartCoroutine(FloatRockToSlot(blueRock, blueRockSlot.position));
        if (redRockSlot != null) yield return StartCoroutine(FloatRockToSlot(redRock, redRockSlot.position));

        yield return new WaitForSeconds(0.2f); // Short delay before opening the door
        StartCoroutine(OpenDoor());
    }

    private IEnumerator FloatRockToSlot(GameObject rock, Vector3 targetPosition)
    {
        Vector3 startPosition = player.position + Vector3.up * 1.5f; // Start slightly above the player
        float elapsedTime = 0f;
        float duration = 0.5f; // Fast movement

        while (elapsedTime < duration)
        {
            rock.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rock.transform.position = targetPosition;
    }

    private IEnumerator OpenDoor()
    {
        Vector3 leftStartPos = leftDoor.position;
        Vector3 rightStartPos = rightDoor.position;
        Vector3 leftTargetPos = leftStartPos + Vector3.left * doorOpenDistance;
        Vector3 rightTargetPos = rightStartPos + Vector3.right * doorOpenDistance;

        float elapsedTime = 0;
        float duration = 0.5f; // Faster door opening

        while (elapsedTime < duration)
        {
            leftDoor.position = Vector3.Lerp(leftStartPos, leftTargetPos, elapsedTime / duration);
            rightDoor.position = Vector3.Lerp(rightStartPos, rightTargetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leftDoor.position = leftTargetPos;
        rightDoor.position = rightTargetPos;
        Debug.Log("Door opened!");
    }
}
