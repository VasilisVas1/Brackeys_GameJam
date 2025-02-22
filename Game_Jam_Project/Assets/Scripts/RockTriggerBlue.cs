using UnityEngine;

public class RockTriggerBlue : MonoBehaviour
{
    public StandingBlueHeadRockController rockController;
    //TEST TEST TEST TEST
    public GameObject redTrigger, whiteTrigger;

    private void OnTriggerEnter(Collider other)
    {

        if (!enabled) return; // Check if the script is enabled

        if (other.CompareTag("Player") && rockController != null)
        {
            //TEST TEST TEST TEST
            redTrigger.SetActive(false);
            whiteTrigger.SetActive(false);
            //TEST TEST TEST TEST

            rockController.TriggerEmerge();
        }
    }
}
