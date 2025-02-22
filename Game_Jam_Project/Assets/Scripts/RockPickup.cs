using UnityEngine;
using UnityEngine.UI;  
using TMPro;           
using System.Collections; // Required for IEnumerator

public class RockPickup : MonoBehaviour
{
    private bool isCarryingRock = false;
    private GameObject carriedRock;
    private GameObject currentRock;
    private GameObject currentPlacementPoint;
    public float placementOffsetZ = 0.5f; 

    // UI Elements
    public GameObject pickUpText;  
    public GameObject placeText;   

    // Audio
    public AudioSource audioSource;
    public AudioClip pickUpSound;
    public AudioClip placeSound;
    
    // Narration
    public AudioClip narrationClip;
    public GameObject speechBubble;
    public TMP_Text subtitleText;
    public string subtitleTextContent;
    public RectTransform bubbleRectTransform;

    private int placedRockCount = 0;  // Track placed rocks

    void Start()
    {
        if (pickUpText) pickUpText.SetActive(false);
        if (placeText) placeText.SetActive(false);
        if (speechBubble) speechBubble.SetActive(false);
    }

    void Update()
    {
        if (!isCarryingRock && currentRock != null && Input.GetKeyDown(KeyCode.E))
        {
            PickUpRock();
        }

        if (isCarryingRock && currentPlacementPoint != null && Input.GetKeyDown(KeyCode.E))
        {
            PlaceRock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return; 

        if (other.CompareTag("Rock") && !isCarryingRock)
        {
            currentRock = other.gameObject;
            if (pickUpText) pickUpText.SetActive(true);
        }

        if (other.CompareTag("PlacementPoint") && isCarryingRock)
        {
            currentPlacementPoint = other.gameObject;
            if (placeText) placeText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;

        if (other.gameObject == currentRock)
        {
            currentRock = null;
            if (pickUpText) pickUpText.SetActive(false);
        }

        if (other.gameObject == currentPlacementPoint)
        {
            currentPlacementPoint = null;
            if (placeText) placeText.SetActive(false);
        }
    }

    void PickUpRock()
    {
        carriedRock = currentRock;
        carriedRock.SetActive(false);
        isCarryingRock = true;
        currentRock = null;

        if (pickUpText) pickUpText.SetActive(false);
        if (audioSource && pickUpSound) audioSource.PlayOneShot(pickUpSound);
    }

    void PlaceRock()
    {
        Vector3 newPosition = currentPlacementPoint.transform.position;
        newPosition.z += placementOffsetZ; 

        carriedRock.transform.position = newPosition;
        carriedRock.SetActive(true);

        Collider rockCollider = carriedRock.GetComponent<Collider>();
        if (rockCollider) rockCollider.isTrigger = false;

        carriedRock.tag = "PlacedRock";
        currentPlacementPoint.tag = "UsedPlacementPoint";

        isCarryingRock = false;
        carriedRock = null;
        currentPlacementPoint = null;

        placedRockCount++; // Increment rock placement count

        if (placeText) placeText.SetActive(false);
        if (audioSource && placeSound) audioSource.PlayOneShot(placeSound);

        // Trigger narration when the third rock is placed
        if (placedRockCount == 3)
        {
            StartCoroutine(PlayNarration());
        }
    }

    private IEnumerator PlayNarration()
    {
        if (narrationClip != null && audioSource != null && speechBubble != null)
        {
            speechBubble.SetActive(true);
            subtitleText.text = subtitleTextContent;
            LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleRectTransform);

            audioSource.clip = narrationClip;
            audioSource.Play();

            yield return new WaitForSeconds(narrationClip.length);

            subtitleText.text = "";
            speechBubble.SetActive(false);
        }
    }
}
