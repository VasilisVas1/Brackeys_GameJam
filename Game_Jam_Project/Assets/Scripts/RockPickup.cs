using UnityEngine;
using UnityEngine.UI;  // Required for UI elements
using TMPro;           // If using TextMeshPro

public class RockPickup : MonoBehaviour
{
    private bool isCarryingRock = false;
    private GameObject carriedRock;
    private GameObject currentRock;
    private GameObject currentPlacementPoint;
    public float placementOffsetZ = 0.5f; // Adjusts rock placement

    // UI Elements
    public GameObject pickUpText;  // Assign in Inspector
    public GameObject placeText;   // Assign in Inspector

    // Audio
    public AudioSource audioSource;
    public AudioClip pickUpSound;
    public AudioClip placeSound;

    void Start()
    {
        // Hide UI at the start
        if (pickUpText) pickUpText.SetActive(false);
        if (placeText) placeText.SetActive(false);
    }

    void Update()
    {
        // Pick up rock
        if (!isCarryingRock && currentRock != null && Input.GetKeyDown(KeyCode.E))
        {
            PickUpRock();
        }

        // Place rock
        if (isCarryingRock && currentPlacementPoint != null && Input.GetKeyDown(KeyCode.E))
        {
            PlaceRock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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

        if (pickUpText) pickUpText.SetActive(false); // Hide UI
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

        if (placeText) placeText.SetActive(false); // Hide UI
        if (audioSource && placeSound) audioSource.PlayOneShot(placeSound);
    }
}
