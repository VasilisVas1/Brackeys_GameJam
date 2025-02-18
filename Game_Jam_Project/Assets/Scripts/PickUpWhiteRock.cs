using UnityEngine;
using UnityEngine.UI;

public class PickUpWhiteRock : MonoBehaviour
{
    private bool playerInRange = false;
    public GameObject uiMessage; // Assign a UI Text object in the Inspector

    void Start()
    {
        uiMessage.SetActive(false); // Hide message initially
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            uiMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiMessage.SetActive(false);
        }
    }

    private void PickUp()
    {
        Debug.Log("Player picked up the White Rock!");
        uiMessage.SetActive(false);
        Destroy(gameObject); // Remove the reward from the scene
    }
}
