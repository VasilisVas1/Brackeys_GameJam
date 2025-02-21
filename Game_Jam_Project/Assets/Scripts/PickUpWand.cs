using UnityEngine;
using TMPro;

public class PickUpWand : MonoBehaviour
{
    public Transform rightHand; 
    public GameObject interactionText; 

    private bool isPlayerInRange = false;
    private TMP_Text interactionTextComponent;

    void Start()
    {
        if (interactionText != null)
        {
            interactionTextComponent = interactionText.GetComponent<TMP_Text>(); 
            interactionText.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionTextComponent != null)
            {
                interactionTextComponent.text = "Press E to Pick Up Wand";
                interactionText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionText != null)
            {
                interactionText.SetActive(false);
            }
        }
    }

    private void PickUp()
    {
        transform.SetParent(rightHand);
        transform.localPosition = new Vector3(0.1f, -0.3f, 0.3f); // Adjust as needed
        transform.localRotation = Quaternion.Euler(0, 90, 0); // Adjust as needed
        
        if (interactionText != null)
        {
            interactionText.SetActive(false);
        }

        WandShooting wandShooting = GetComponent<WandShooting>();
if (wandShooting != null)
{
    wandShooting.EnableShooting();
}

    MonsterSpawner spawner = FindObjectOfType<MonsterSpawner>();
    if (spawner != null)
    {
        spawner.StartWave();
    }
        
        GetComponent<Collider>().enabled = false;
    }
}
