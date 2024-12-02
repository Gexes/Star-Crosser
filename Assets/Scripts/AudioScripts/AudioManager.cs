using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip buttonClickSound;           // Sound for button clicks
    [SerializeField] private AudioClip[] backgroundMusicClips;     // Array of background music clips to cycle through
    [SerializeField] private AudioClip[] spacelogClips;            // Array of SpaceLog audio clips
    [SerializeField] private AudioSource audioSource;             // Reference to the AudioSource
    [SerializeField] private Animator animator;                  // Reference to the Animator (for triggering animation)

    private int currentTrackIndex = 0; // Keeps track of the current background music track

    private void Awake()
    {
        // Ensure an AudioSource is attached to this object
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start playing background music if there are any tracks in the array
        if (backgroundMusicClips != null && backgroundMusicClips.Length > 0)
        {
            AudioPlayer();
        }
        else
        {
            Debug.LogWarning("No background music clips assigned.");
        }

        // Start playing SpaceLog clips automatically
        if (spacelogClips != null && spacelogClips.Length > 0)
        {
            StartCoroutine(CycleSpaceLogClips());
        }
        else
        {
            Debug.LogWarning("No SpaceLog clips assigned.");
        }
    }

    /// <summary>
    /// Play the button click sound.
    /// </summary>
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("Button click sound not assigned.");
        }
    }

    /// <summary>
    /// Starts cycling through the background music clips on a loop.
    /// </summary>
    public void AudioPlayer()
    {
        if (backgroundMusicClips != null && backgroundMusicClips.Length > 0)
        {
            // Pick a random track from the array
            currentTrackIndex = Random.Range(0, backgroundMusicClips.Length);
            PlayBackgroundMusic(backgroundMusicClips[currentTrackIndex]);

            // Start cycling through tracks after the current track ends
            StartCoroutine(CycleMusicTracks());
        }
        else
        {
            Debug.LogWarning("No background music clips assigned to cycle.");
        }
    }

    /// <summary>
    /// Coroutine that loops through the background music clips.
    /// </summary>
    private IEnumerator CycleMusicTracks()
    {
        while (true)
        {
            // Wait for the current music to finish before switching to the next track
            yield return new WaitForSeconds(audioSource.clip.length);

            // Pick a random track from the array
            currentTrackIndex = Random.Range(0, backgroundMusicClips.Length);
            PlayBackgroundMusic(backgroundMusicClips[currentTrackIndex]);
        }
    }

    private IEnumerator CycleSpaceLogClips()
    {
        int index = 0;
        bool isTrackPlaying = false;  // Flag to track whether a track is playing

        while (spacelogClips != null && spacelogClips.Length > 0)
        {
            // Play the current SpaceLog clip
            AudioClip clip = spacelogClips[index];
            if (clip != null)
            {
                // Start playing the audio clip
                audioSource.clip = clip;
                audioSource.loop = false;
                audioSource.Play();

                // Trigger PlayAnimation when the clip starts
                if (animator != null)
                {
                    animator.SetTrigger("PlayAnimation");
                }

                isTrackPlaying = true;  // Set the flag to true since a track is playing

                // Wait for the clip to finish playing
                yield return new WaitForSeconds(clip.length);

                // Ensure the StopAnimation trigger is sent when the clip finishes
                if (animator != null)
                {
                    animator.SetTrigger("StopAnimation");
                }

                isTrackPlaying = false;  // Reset the flag after the track finishes
            }
            else
            {
                Debug.LogWarning($"SpaceLog clip at index {index} is null.");
            }

            // Trigger the Delay animation to handle the 15-second wait
            if (animator != null)
            {
                animator.SetTrigger("Delay");  // Trigger the Delay animation
            }

            // Wait for 15 seconds (or any other specified time for the delay)
            yield return new WaitForSeconds(15f);

            // After the delay, trigger the PlayAnimation again for the next clip
            if (animator != null)
            {
                animator.SetTrigger("PlayAnimation");
            }

            // Move to the next clip, loop back to the start if at the end
            index = (index + 1) % spacelogClips.Length;
        }

        // Ensure animation is idle if no tracks are left to play
        if (animator != null)
        {
            animator.SetTrigger("StopAnimation");
        }
    }












    /// <summary>
    /// Play the selected background music clip.
    /// </summary>
    private void PlayBackgroundMusic(AudioClip newMusicClip)
    {
        if (newMusicClip != null)
        {
            audioSource.clip = newMusicClip;
            audioSource.loop = false;  // Ensure it does not loop automatically
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("New background music clip is null.");
        }
    }

    /// <summary>
    /// Play a random SpaceLog audio clip and trigger animation.
    /// </summary>
    public void SpaceLog()
    {
        if (spacelogClips != null && spacelogClips.Length > 0)
        {
            // Select a random clip
            AudioClip randomClip = spacelogClips[Random.Range(0, spacelogClips.Length)];

            // Play the selected clip
            if (randomClip != null)
            {
                audioSource.clip = randomClip;
                audioSource.loop = false;
                audioSource.Play();

                // Trigger the animation
                if (animator != null)
                {
                    animator.SetTrigger("PlayAnimation");
                }
                else
                {
                    Debug.LogWarning("Animator is not assigned.");
                }

                // Stop the animation once the audio is finished
                StartCoroutine(StopAnimationWhenDone(randomClip.length));
            }
            else
            {
                Debug.LogWarning("Selected SpaceLog clip is null.");
            }
        }
        else
        {
            Debug.LogWarning("No SpaceLog clips assigned.");
        }
    }

    /// <summary>
    /// Stops the animation once the audio clip finishes.
    /// </summary>
    private IEnumerator StopAnimationWhenDone(float duration)
    {
        // Wait for the duration of the audio clip
        yield return new WaitForSeconds(duration);

        // Trigger the StopAnimation to revert to the idle state
        if (animator != null)
        {
            animator.SetTrigger("StopAnimation");
        }
        else
        {
            Debug.LogWarning("Animator is not assigned.");
        }
    }

    /// <summary>
    /// Stop the background music.
    /// </summary>
    public void StopBackgroundMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Set the volume of the audio.
    /// </summary>
    /// <param name="volume">Volume level (0 to 1).</param>
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    /// <summary>
    /// Mute or unmute the audio.
    /// </summary>
    /// <param name="mute">True to mute, false to unmute.</param>
    public void SetMute(bool mute)
    {
        audioSource.mute = mute;
    }
}
