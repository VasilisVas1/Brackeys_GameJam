using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class NarratorTrigger2 : MonoBehaviour
{
    public AudioClip narrationClip;
    public string subtitleTextContent;
    public TMP_Text subtitleText;
    public AudioSource audioSource;
    public GameObject speechBubble; // Reference to the speech bubble UI
    public RectTransform bubbleRectTransform; // The RectTransform of the speech bubble
    public MonoBehaviour RockTrigger,RockTriggerBlue,RockTriggerRed;
    //public MonoBehaviour StandingBlueHeadRockController,StandingRedHeadRockController,StandingWhiteHeadRockController;


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
        
        // Set initial subtitle text
        subtitleText.text = subtitleTextContent;

        // Force UI to update its size
        LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleRectTransform);

        // Play audio
        audioSource.clip = narrationClip;
        audioSource.Play();

        // Wait for a few seconds before changing the text
        yield return new WaitForSeconds(15f); // Adjust this delay as needed

        // Change subtitle text
        subtitleText.text = "Ah, yes. The classic trio. The Blue Guardian, the Red Guardian, and the White Guardian. And guess what? They each have a shiny, important-looking rock. I don’t think they’re just going to give them to you, though..."; 

        // Force UI update
        LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleRectTransform);

        // Wait for the remaining audio duration
        float remainingTime = narrationClip.length - 15f; 
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // Clear text and hide speech bubble
        subtitleText.text = "";
        speechBubble.SetActive(false);

        // Enable triggers
        RockTrigger.enabled = true;
        RockTriggerBlue.enabled = true;
        RockTriggerRed.enabled = true;
       // StandingBlueHeadRockController.enabled = true;
       // StandingRedHeadRockController.enabled = true;
       // StandingWhiteHeadRockController.enabled = true;

        // Disable collider to prevent re-triggering
        GetComponent<Collider>().enabled = false;
    }
}

}
