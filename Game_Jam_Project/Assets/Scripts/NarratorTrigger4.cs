using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NarratorTrigger4 : MonoBehaviour
{
    public AudioClip narrationClip;
    public string subtitleTextContent;
    public TMP_Text subtitleText;
    public AudioSource audioSource;
    public GameObject speechBubble;
    public RectTransform bubbleRectTransform;
    public Image blackScreen;
    public Transform playerCamera;
    public Light directionalLight; // Reference to the sun light
    public float lookDistance = 20f;
    public float blackoutDuration = 10f;
    public float lightFadeDuration = 5f; // How long to fade the light

    private bool hasTriggered = false;
    private float originalLightIntensity;
    public AudioSource background;
    public MonoBehaviour FirstPersonController;

    private void Start()
    {
        blackScreen.gameObject.SetActive(false); // Start disabled
        blackScreen.color = new Color(0, 0, 0, 0); // Fully transparent

        if (directionalLight != null)
        {
            originalLightIntensity = directionalLight.intensity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
             // Remove the clip from the audio source
            background.clip = null;
            StartCoroutine(PlayNarration());
            StartCoroutine(DisableLeftClick());

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

            // Start dimming the light while narration plays
            StartCoroutine(FadeOutLight());

            yield return new WaitForSeconds(narrationClip.length);

            subtitleText.text = "";
            speechBubble.SetActive(false);

            StartCoroutine(StartDisappearanceEffect());
        }
    }

    private IEnumerator StartDisappearanceEffect()
    {
        float elapsedTime = 0;
        float effectDuration = blackoutDuration - 2f;

        while (elapsedTime < effectDuration)
        {
            elapsedTime += Time.deltaTime;
            RaycastForObjects();
            yield return null;
        }

        StartCoroutine(BlinkingToBlack());
    }

    private void RaycastForObjects()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, lookDistance))
        {
            GameObject targetObject = hit.collider.gameObject;

            if (!targetObject.CompareTag("Player") && targetObject != this.gameObject)
            {
                targetObject.SetActive(false);
            }
        }
    }

    private IEnumerator BlinkingToBlack()
    {
        blackScreen.gameObject.SetActive(true);

        float[] blinkTimings = { 0.3f, 0.5f, 0.7f, 1f }; // Times when blinks happen

        foreach (float blinkTime in blinkTimings)
        {
            yield return BlinkEffect(blinkTime);
        }

        yield return FullFadeToBlack();
    }

    private IEnumerator BlinkEffect(float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / (duration / 2)));
            yield return null;
        }

        elapsedTime = 0;

        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackScreen.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsedTime / (duration / 2)));
            yield return null;
        }
    }

    private IEnumerator FullFadeToBlack()
    {
        float elapsedTime = 0;
        float fadeDuration = 3f;
        FirstPersonController.enabled=false;


        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            blackScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / fadeDuration));
            yield return null;
        }

        yield return new WaitForSeconds(blackoutDuration);
        EndGame();
    }

    private IEnumerator FadeOutLight()
    {
        float elapsedTime = 0;

        while (elapsedTime < lightFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (directionalLight != null)
            {
                directionalLight.intensity = Mathf.Lerp(originalLightIntensity, 0, elapsedTime / lightFadeDuration);
            }
            yield return null;
        }

        if (directionalLight != null)
        {
            directionalLight.intensity = 0; // Ensure it is fully dark
        }
    }

    private IEnumerator DisableLeftClick()
{
    while (true) // Keep blocking left-click until the scene changes
    {
        if (Input.GetMouseButtonDown(0)) // 0 = Left Click
        {
            yield return null; // Ignore input
        }
        yield return null;
    }
}


   private void EndGame()
{
    Time.timeScale = 1f; // Ensure time is running when returning to main menu
    Cursor.lockState = CursorLockMode.None; // Unlock cursor
    Cursor.visible = true; // Make cursor visible
    SceneManager.LoadScene("MainMenu"); // Change to your actual main menu scene name
}

}
