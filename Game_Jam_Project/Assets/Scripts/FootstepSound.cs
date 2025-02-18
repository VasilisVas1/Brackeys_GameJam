using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class FootstepSound : MonoBehaviour
    {
        public AudioClip[] footstepSounds; // Array of footstep audio clips
        public float stepDistance = 2f; // Distance player must travel to trigger a step sound
        public LayerMask terrainLayer; // Set this to the terrain layer in Unity
        public float raycastDistance = 3f; // Distance for the raycast to check (adjust as needed)

        private AudioSource audioSource;
        private Vector3 lastPosition;
        private float distanceTraveled;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            lastPosition = transform.position;
            distanceTraveled = 0f;
        }

        private void Update()
        {
            // Check if the player is on the terrain and if movement should be tracked
            if (IsOnTerrain())
            {
                TrackMovementAndPlayFootsteps();
            }
        }

        private void TrackMovementAndPlayFootsteps()
        {
            // Calculate the distance moved since the last frame
            Vector3 currentPosition = transform.position;
            distanceTraveled += Vector3.Distance(currentPosition, lastPosition);

            // If the distance traveled exceeds the step distance, play a footstep sound
            if (distanceTraveled >= stepDistance)
            {
                PlayFootstepSound();
                distanceTraveled = 0f; // Reset the distance tracker
            }

            // Update the last position
            lastPosition = currentPosition;
        }

        private void PlayFootstepSound()
{
    if (footstepSounds.Length > 0)
    {
        AudioClip footstepClip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        audioSource.PlayOneShot(footstepClip, 0.15f); // Adjust 0.3f to a lower/higher value
    }
}


        // Check if the player is on the terrain using a raycast
        private bool IsOnTerrain()
        {
            RaycastHit hit;
            // Cast the ray from a slightly higher point to avoid missing terrain on uneven surfaces
            Vector3 rayStart = transform.position + Vector3.up * 0.5f; // Adjust offset as needed

            // Cast a ray downward to check if the player is on the terrain
            if (Physics.Raycast(rayStart, Vector3.down, out hit, raycastDistance, terrainLayer))
            {
                return true; // Player is on the terrain
            }
            return false; // Player is not on the terrain
        }
    }