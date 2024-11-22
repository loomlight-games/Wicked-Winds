using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
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



    public void PlayAudio(AudioClip clip, float fadeDuration = 1f)
    {
        StartCoroutine(FadeAudio(clip, fadeDuration));
    }

    private IEnumerator FadeAudio(AudioClip newClip, float duration)
    {
        if (audioSource.isPlaying)
        {
            // Fading out the current audio
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                audioSource.volume = 1 - (t / duration);
                yield return null;
            }
            audioSource.Stop();
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fading in the new audio
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = t / duration;
            yield return null;
        }

        audioSource.volume = 0.5f; // Ensure max volume
    }
}
