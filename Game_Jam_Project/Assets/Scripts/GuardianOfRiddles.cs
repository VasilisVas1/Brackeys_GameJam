using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuardianOfRiddles : MonoBehaviour
{
    public GameObject uiMessage; // "Press E to start the Riddle"
    public TextMeshProUGUI riddleText; // Text to display the riddle
    public TMP_InputField answerInput; // Input field for player's answer
    public Button submitButton; // Button to submit the answer
    public GameObject rewardObject; // The reward object

    public AudioSource riddleAudio; // Audio of the riddle
    public AudioSource successAudio; // Audio for correct answer
    public AudioSource failureAudio; // Audio for wrong answer

    public string correctAnswer; // The correct answer for the riddle
    public MonoBehaviour FirstPersonController;

    private bool playerInRange = false;
    private bool riddleStarted = false;

    void Start()
    {
        uiMessage.SetActive(false);
        riddleText.gameObject.SetActive(false);
        answerInput.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        rewardObject.SetActive(false);

        submitButton.onClick.AddListener(CheckAnswer);
    }

    void Update()
    {
        if (playerInRange && !riddleStarted && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartRiddle());
        }
    }

    IEnumerator StartRiddle()
{
    riddleStarted = true;
    uiMessage.SetActive(false);

    // Play riddle audio
    if (riddleAudio != null)
    {
        riddleAudio.Play();
        yield return new WaitForSeconds(riddleAudio.clip.length);
    }

    // Unlock and show cursor so player can interact with UI
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    FirstPersonController.enabled=false;

    // Show text and input
    riddleText.gameObject.SetActive(true);
    riddleText.text = "I grow shorter as I grow older, yet I bring light to the darkest nights. What am I?"; // Change to actual riddle text
    answerInput.gameObject.SetActive(true);
    submitButton.gameObject.SetActive(true);
}

    public void CheckAnswer()
    {
        string playerAnswer = answerInput.text.Trim().ToLower();
        string correct = correctAnswer.Trim().ToLower();

        if (playerAnswer == correct)
        {
            StartCoroutine(HandleCorrectAnswer());
        }
        else
        {
            StartCoroutine(HandleWrongAnswer());
        }
    }

IEnumerator HandleCorrectAnswer()
{
    answerInput.gameObject.SetActive(false);
    submitButton.gameObject.SetActive(false);
    riddleText.gameObject.SetActive(false);

    // Hide cursor and lock it back to game
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    if (successAudio != null)
    {
        successAudio.Play();
        yield return new WaitForSeconds(successAudio.clip.length);
    }

    
    FirstPersonController.enabled=true;

    rewardObject.SetActive(true);
}

IEnumerator HandleWrongAnswer()
{
    answerInput.text = ""; // Clear input
    riddleText.text = "Wrong! Try again.";

    if (failureAudio != null)
    {
        failureAudio.Play();
        yield return new WaitForSeconds(failureAudio.clip.length);
    }

    riddleText.text = "I grow shorter as I grow older, yet I bring light to the darkest nights. What am I?";
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            uiMessage.SetActive(!riddleStarted);
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
}
