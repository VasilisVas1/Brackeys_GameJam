using System.Collections;
using UnityEngine;

public class StandingWhiteHeadRockController : MonoBehaviour
{
    public Transform leftHand, rightHand;
    public GameObject triggerObject;
    public GameObject leftfoot, rightfoot;
    public GameObject mushroom1, mushroom2, mushroom3, mushroom4, mushroom5;
    public MonoBehaviour GuardianMemoryPuzzle;
    public AudioSource emergeSound; // AudioSource for the sound effect

    public float emergeHeight = 1f;
    public float emergeSpeed = 2f;
    public float breathingIntensity = 0.1f; // Small breathing movement
    public float breathingSpeed = 1f;
    public float limbMoveAmount = 0.05f; // Small hand movement
    public float limbMoveSpeed = 1.5f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 leftHandStart, rightHandStart;
    private bool hasEmerged = false;

    void Start()
    {
        mushroom1.SetActive(false);
        mushroom2.SetActive(false);
        mushroom3.SetActive(false);
        mushroom4.SetActive(false);
        mushroom5.SetActive(false);
        GuardianMemoryPuzzle.enabled = false;

        // Store initial position
        initialPosition = transform.position;
        targetPosition = initialPosition + new Vector3(0, emergeHeight, 0);

        // Store original hand positions
        if (leftHand) leftHandStart = leftHand.localPosition;
        if (rightHand) rightHandStart = rightHand.localPosition;
    }

    void Update()
    {
        if (hasEmerged)
        {
            // Breathing effect: slight up & down movement
            float breathOffset = Mathf.Sin(Time.time * breathingSpeed) * breathingIntensity;
            transform.position = targetPosition + new Vector3(0, breathOffset, 0);

            // Move only the hands slightly
            float limbOffset = Mathf.Sin(Time.time * limbMoveSpeed) * limbMoveAmount;
            if (leftHand) leftHand.localPosition = leftHandStart + new Vector3(0, limbOffset, 0);
            if (rightHand) rightHand.localPosition = rightHandStart + new Vector3(0, limbOffset, 0);
        }
    }

    private IEnumerator Emerge()
    {
        float elapsedTime = 0f;
        float duration = (targetPosition.y - initialPosition.y) / emergeSpeed;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        hasEmerged = true;

        leftfoot.SetActive(true);
        rightfoot.SetActive(true);
        mushroom1.SetActive(true);
        mushroom2.SetActive(true);
        mushroom3.SetActive(true);
        mushroom4.SetActive(true);
        mushroom5.SetActive(true);

        // Play the emerge sound and ensure GuardianMemoryPuzzle enables only after the sound finishes
        if (emergeSound && emergeSound.clip)
        {
            emergeSound.Play();
            yield return new WaitForSeconds(emergeSound.clip.length); // Strict wait for the full clip duration
        }

        // Now enable the GuardianMemoryPuzzle
        GuardianMemoryPuzzle.enabled = true;
    }

    public void TriggerEmerge()
    {
        if (!hasEmerged)
        {
            StartCoroutine(Emerge());
        }
    }
}
