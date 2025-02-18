using System.Collections;
using UnityEngine;

public class StandingYellowHeadRockController : MonoBehaviour
{
    public Transform leftHand, rightHand;
    public GameObject triggerObject;
    public GameObject leftfoot,rightfoot;

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
        leftfoot.SetActive(true);
        rightfoot.SetActive(true);
        transform.position = targetPosition;
        hasEmerged = true;
    }

    public void TriggerEmerge()
    {
        if (!hasEmerged)
        {
            StartCoroutine(Emerge());
        }
    }
}
