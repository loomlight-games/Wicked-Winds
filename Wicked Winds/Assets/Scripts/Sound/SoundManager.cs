using System.Collections;
using UnityEngine;

/// <summary>
/// Handles music and sound effects reproduction
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public float volume = 0.6f, 
                fadeDuration = 2f;

    [SerializeField] private AudioClip[] soundEffects;
    [SerializeField] private AudioClip[] musicTracks;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(int id)
    {
        audioSource.PlayOneShot (soundEffects[id], volume);
    }

    public void PlayMusicTrack(int id)
    {
        // Return if it's already playing
        if(audioSource.clip == musicTracks[id]) return;

        StartCoroutine(FadeAudio(musicTracks[id]));
    }

    private IEnumerator FadeAudio(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            // Fading out the current audio
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = 1 - (t / fadeDuration);
                yield return null;
            }
            audioSource.Stop();
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fading in the new audio
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = t / fadeDuration;
            yield return null;
        }

        audioSource.volume = volume; // Ensure max volume
    }
}
