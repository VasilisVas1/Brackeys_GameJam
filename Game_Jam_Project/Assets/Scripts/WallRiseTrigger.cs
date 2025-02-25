using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class WallRiseTrigger : MonoBehaviour
{
    public Transform wall; // Drag your wall object here
    public Vector3 finalPosition; // The final position of the wall
    public float riseDuration = 2f; // How long the wall takes to rise
    public Camera playerCamera;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;
    
    public GameObject player; // Drag your player here
    private RockPickup rockPickup;

    public AudioSource wallRiseSound; // 🔊 Drag & drop AudioSource here!

    private Vector3 startPosition;
    private bool hasTriggered = false;


    public AudioClip narrationClip;
    public string subtitleTextContent;
    public TMP_Text subtitleText;
    public AudioSource audioSource;
    public GameObject speechBubble; // Reference to the speech bubble UI
    public RectTransform bubbleRectTransform; // The RectTransform of the speech bubble

    void Start()
    {
        startPosition = wall.position; // Save starting position
        
        // ✅ Get RockPickup script from Player
        if (player != null)
        {
            rockPickup = player.GetComponent<RockPickup>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.gameObject == player) // Check if player triggered
        {
            hasTriggered = true;
            
            // 🔊 Play sound effect when the wall starts rising
            if (wallRiseSound != null)
            {
                wallRiseSound.Play();
            }

            StartCoroutine(RaiseWall());
            StartCoroutine(ShakeCamera());

        }
    }

   IEnumerator RaiseWall()
{
    float elapsedTime = 0;

    while (elapsedTime < riseDuration)
    {
        wall.position = Vector3.Lerp(startPosition, finalPosition, elapsedTime / riseDuration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    wall.position = finalPosition; // Ensure wall reaches the correct position

    // Wait for the sound to finish before starting narration
    if (wallRiseSound != null)
    {
        yield return new WaitForSeconds(wallRiseSound.clip.length-20);
    }

    // Start narration
    StartCoroutine(PlayNarration());
}

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

            // ✅ Enable RockPickup script
            if (rockPickup != null)
            {
                rockPickup.enabled = true;
            }

            // Clear text and hide speech bubble
            subtitleText.text = "";
            speechBubble.SetActive(false);

            // Disable collider to prevent re-triggering
            GetComponent<Collider>().enabled = false;
        }
    }


    IEnumerator ShakeCamera()
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

        playerCamera.transform.localPosition = originalPosition; // Reset camera position
    }
}
