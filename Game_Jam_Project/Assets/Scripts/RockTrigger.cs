using UnityEngine;

public class RockTrigger : MonoBehaviour
{
    public StandingWhiteHeadRockController rockController;
    //TEST TEST TEST TEST
    public GameObject redTrigger, blueTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return; // Check if the script is enabled

        if (other.CompareTag("Player") && rockController != null)
        {
            //TEST TEST TEST TEST
            redTrigger.SetActive(false);
            blueTrigger.SetActive(false);
            //TEST TEST TEST TEST
            rockController.TriggerEmerge();
        }
    }
}
