using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuardianOfBeasts : MonoBehaviour
{
    public GameObject beast; // The beast object
    public GameObject rewardObject; // Reward after catching the beast
    public GameObject uiMessage; // UI: "Press E to start the challenge"
    public AudioSource challengeAudio; // Guardian's challenge audio
    public AudioSource successAudio; // Guardian's success audio
    public AudioSource jumpAudio; // Jump sound effect

    public float minX, maxX, minZ, maxZ; // Movement boundaries
    public float jumpHeight = 1.5f;
    public float jumpDistance = 2.5f;
    public float jumpSpeed = 0.15f;
    public int jumpsPerMove = 5;
    public Transform player; // Reference to the player for distance calculation
    public float maxSoundDistance = 10f; // Max distance where the sound is audible

    private bool challengeStarted = false;
    private bool playerInGuardianZone = false;
    private Vector3 lastPosition;

    public GameObject blueTrigger, whiteTrigger,redTrigger;

    public GameObject leftEye,rightEye;
    public Material newMaterial; // Assign this in the Inspector

    //test test test
    public TMP_Text subtitleText;
    public GameObject speechBubble; // Reference to the speech bubble UI
    public RectTransform bubbleRectTransform; // The RectTransform of the speech bubble
        public string subtitleTextContent;



    void Start()
    {
        beast.SetActive(false);
        uiMessage.SetActive(false);
        rewardObject.SetActive(false);
        lastPosition = beast.transform.position;
    }

    void Update()
    {
        if (playerInGuardianZone && !challengeStarted && Input.GetKeyDown(KeyCode.E))
        {
            StartChallenge();
        }
    }

   private bool challengeCompleted = false; // Prevents restarting the challenge

void StartChallenge()
{
    if (challengeCompleted) return; // Prevents restarting the challenge

    challengeStarted = true;
    uiMessage.SetActive(false);

//TEST TEST TEST
            speechBubble.SetActive(true);
            
            // Set subtitle text
            subtitleText.text = subtitleTextContent;

            // Force UI to update its size
            LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleRectTransform);
            //TEST TEST TEST


    challengeAudio.Play();
    StartCoroutine(BeastMovementRoutine());
}

    IEnumerator BeastMovementRoutine()
    {
        yield return new WaitForSeconds(challengeAudio.clip.length);
        subtitleText.text = "";
    speechBubble.SetActive(false);
        beast.SetActive(true);

        while (challengeStarted)
        {
            Vector3 targetPosition = GetRandomPosition();

            for (int i = 0; i < jumpsPerMove; i++)
            {
                Vector3 jumpTarget = beast.transform.position + (targetPosition - beast.transform.position).normalized * jumpDistance;
                jumpTarget = new Vector3(
                    Mathf.Clamp(jumpTarget.x, minX, maxX),
                    beast.transform.position.y,
                    Mathf.Clamp(jumpTarget.z, minZ, maxZ)
                );

                RotateTowards(jumpTarget);
                PlayJumpSound();
                yield return StartCoroutine(JumpToPosition(jumpTarget));
            }
        }
    }

    IEnumerator JumpToPosition(Vector3 target)
    {
        Vector3 start = beast.transform.position;
        float elapsedTime = 0f;
        float jumpTime = jumpSpeed;

        while (elapsedTime < jumpTime)
        {
            float progress = elapsedTime / jumpTime;
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            beast.transform.position = Vector3.Lerp(start, target, progress) + Vector3.up * height;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        beast.transform.position = target;
        lastPosition = target; // Update last position
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomPos;
        do
        {
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            randomPos = new Vector3(x, beast.transform.position.y, z);
        }
        while (Vector3.Distance(randomPos, lastPosition) < jumpDistance * 1.5f); // Ensures no immediate backtracking

        return randomPos;
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - beast.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            beast.transform.rotation = targetRotation;
        }
    }

    void PlayJumpSound()
    {
        if (jumpAudio != null && player != null)
        {
            float distance = Vector3.Distance(beast.transform.position, player.position);
            float volume = Mathf.Clamp01(1 - (distance / maxSoundDistance)); // Volume decreases with distance
            jumpAudio.volume = volume;
            jumpAudio.Play();
        }
    }

    public void CatchBeast()
{
    challengeStarted = false;
    challengeCompleted = true; // Marks challenge as completed
    jumpAudio = null;
    successAudio.Play();


    speechBubble.SetActive(true);
            
            // Set subtitle text
            subtitleText.text = "The elusive spirit of the mushroom forest is now tamed. As your reward, I bestow upon you the Red Rock—use it to unlock the path ahead.";

            // Force UI to update its size
            LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleRectTransform);
            //TEST TEST TEST
    StartCoroutine(GrantReward());
}

    IEnumerator GrantReward()
    {
        yield return new WaitForSeconds(successAudio.clip.length);
        rewardObject.SetActive(true);


        subtitleText.text = "";
        speechBubble.SetActive(false);


        //TEST TEST TEST TEST
        blueTrigger.SetActive(true);
        whiteTrigger.SetActive(true);
        redTrigger.transform.position = new Vector3(redTrigger.transform.position.x, redTrigger.transform.position.y + 40f, redTrigger.transform.position.z);
        leftEye.GetComponent<Renderer>().material = newMaterial;
        rightEye.GetComponent<Renderer>().material = newMaterial;



        //TEST TEST TEST TEST

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInGuardianZone = true;
            uiMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInGuardianZone = false;
            uiMessage.SetActive(false);
        }
    }
}
