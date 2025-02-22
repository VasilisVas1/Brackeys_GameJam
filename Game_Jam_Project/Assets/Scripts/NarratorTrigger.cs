using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class NarratorTrigger : MonoBehaviour
{
    public AudioClip narrationClip;
    public string subtitleTextContent;
    public TMP_Text subtitleText;
    public AudioSource audioSource;
    public GameObject speechBubble; // Reference to the speech bubble UI
    public RectTransform bubbleRectTransform; // The RectTransform of the speech bubble

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(PlayNarration());
        }
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

            // Clear text and hide speech bubble
            subtitleText.text = "";
            speechBubble.SetActive(false);

            // Disable collider to prevent re-triggering
            GetComponent<Collider>().enabled = false;
        }
    }
}
