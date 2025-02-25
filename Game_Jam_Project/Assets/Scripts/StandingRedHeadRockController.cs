using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StandingRedHeadRockController : MonoBehaviour
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
    public MonoBehaviour GuardianOfBeasts;
    public AudioSource emergeSound;


    public Camera playerCamera;
    public ParticleSystem dirtParticles; // Reference to dirt particle system
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 leftHandStart, rightHandStart;
    private bool hasEmerged = false;

     public TMP_Text subtitleText;
    public GameObject speechBubble; // Reference to the speech bubble UI
    public RectTransform bubbleRectTransform; // The RectTransform of the speech bubble
        public string subtitleTextContent;

    void Start()
    {
        // Store initial position
        GuardianOfBeasts.enabled=false;
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

public AudioSource earthquakeSound; // Reference to earthquake sound

    private IEnumerator Emerge()
    {
        float elapsedTime = 0f;
        float duration = (targetPosition.y - initialPosition.y) / emergeSpeed;

        if (earthquakeSound && earthquakeSound.clip)
        {
            earthquakeSound.Play();
        }
        StartCoroutine(ShakeCamera());

        if (dirtParticles)
        {
            dirtParticles.Play();
        }


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

        
        if (emergeSound && emergeSound.clip)
        {

            //TEST TEST TEST
            speechBubble.SetActive(true);
            
            // Set subtitle text
            subtitleText.text = subtitleTextContent;

            // Force UI to update its size
            LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleRectTransform);
            //TEST TEST TEST
            emergeSound.Play();
            yield return new WaitForSeconds(emergeSound.clip.length);
        }
        subtitleText.text = "";
            speechBubble.SetActive(false);

        GuardianOfBeasts.enabled = true;
        
    }

    public void TriggerEmerge()
    {
        if (!hasEmerged)
        {
            StartCoroutine(Emerge());
        }
    }

     private IEnumerator ShakeCamera()
    {
        Vector3 originalPosition = playerCamera.transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            playerCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.localPosition = originalPosition;
    }
}
