using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class NarratorTrigger3 : MonoBehaviour
{
    public AudioClip narrationClip;
    public string subtitleTextContent;
    public TMP_Text subtitleText;
    public AudioSource audioSource;
    public AudioSource wand;
    public GameObject speechBubble; // Reference to the speech bubble UI
    public RectTransform bubbleRectTransform; // The RectTransform of the speech bubble
    public GameObject objectToAppear; // The object that should appear
    public float delayBeforeAppearance = 3f; // Delay before the object appears
    public MonoBehaviour PickUpWand;

    public bool hasTriggered = false;

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

            // Wait for a few seconds before making the object appear
            yield return new WaitForSeconds(delayBeforeAppearance);

            if (objectToAppear != null)
            {
                objectToAppear.SetActive(true);
                wand.Play();
            }

            // Wait for the full audio to finish
            yield return new WaitForSeconds(narrationClip.length - delayBeforeAppearance);
            PickUpWand.enabled=true;



            // Clear text and hide speech bubble
            subtitleText.text = "";
            speechBubble.SetActive(false);

            // Disable collider to prevent re-triggering
            GetComponent<Collider>().enabled = false;
        }
    }
}
