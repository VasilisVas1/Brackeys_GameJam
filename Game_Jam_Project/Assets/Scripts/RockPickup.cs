using UnityEngine;

public class RockPickup : MonoBehaviour
{
    private bool isCarryingRock = false;
    private GameObject carriedRock;
    private GameObject currentRock;
    private GameObject currentPlacementPoint;
    public float placementOffsetZ = 0.5f; // Adjust this value to move the rock forward

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
        // Detect rock if it's not placed already
        if (other.CompareTag("Rock") && !isCarryingRock)
        {
            currentRock = other.gameObject;
        }

        // Detect placement point if it's not used already
        if (other.CompareTag("PlacementPoint") && isCarryingRock)
        {
            currentPlacementPoint = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove reference when leaving rock or placement point
        if (other.gameObject == currentRock)
        {
            currentRock = null;
        }

        if (other.gameObject == currentPlacementPoint)
        {
            currentPlacementPoint = null;
        }
    }

    void PickUpRock()
    {
        carriedRock = currentRock;
        carriedRock.SetActive(false);  // Hide rock when picked up
        isCarryingRock = true;
        currentRock = null;  // Clear reference
    }

    void PlaceRock()
    {
        // Calculate new position with Z offset
        Vector3 newPosition = currentPlacementPoint.transform.position;
        newPosition.z += placementOffsetZ; // Moves it slightly outward

        carriedRock.transform.position = newPosition;
        carriedRock.SetActive(true);  // Show rock again

        // Disable the trigger so the rock becomes solid
        Collider rockCollider = carriedRock.GetComponent<Collider>();
        if (rockCollider != null)
        {
            rockCollider.isTrigger = false;
        }

        // Change tags so they cannot be reused
        carriedRock.tag = "PlacedRock";
        currentPlacementPoint.tag = "UsedPlacementPoint";

        // Reset variables
        isCarryingRock = false;
        carriedRock = null;
        currentPlacementPoint = null;
    }
}
