using UnityEngine;

public class BeastCatch : MonoBehaviour
{
    public GameObject catchMessage; // UI: "Press E to catch the beast"
    private bool playerNearBeast = false;
    private GuardianOfBeasts guardianScript; // Reference to guardian

    void Start()
    {
        catchMessage.SetActive(false);
        guardianScript = FindObjectOfType<GuardianOfBeasts>(); // Find the guardian script
    }

    void Update()
    {
        if (playerNearBeast && Input.GetKeyDown(KeyCode.E))
        {
            CatchBeast();
        }
    }

    void CatchBeast()
    {
        playerNearBeast = false;
        catchMessage.SetActive(false);
        guardianScript.CatchBeast(); // Call guardian to grant reward
        gameObject.SetActive(false); // Hide the beast
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearBeast = true;
            catchMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearBeast = false;
            catchMessage.SetActive(false);
        }
    }
}
