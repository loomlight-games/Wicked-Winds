using System.Collections;
using UnityEngine;

/// <summary>
/// Handles music and sound effects reproduction
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public float fadeDuration = 2f;

    [SerializeField] AudioClip[] soundEffects;
    [SerializeField] AudioClip[] musicTracks;

    AudioSource audioSource;

    void Awake()
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

    void PlaySoundEffect(int id, float volume)
    {
        // Set the AudioSource to loop
        audioSource.loop = false;
        audioSource.PlayOneShot (soundEffects[id], volume);
    }

    void PlayMusicTrack(int id, float volume)
    {
        // Return if it's already playing
        if (audioSource.clip == musicTracks[id]) return;

        // Set the AudioSource to loop
        audioSource.loop = true;

        // Start fading to the new track
        StartCoroutine(FadeAudio(musicTracks[id], volume));
    }

    IEnumerator FadeAudio(AudioClip newClip,float  volume)
    {
        Debug.LogWarning("Change song");
        
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

    //////////////////////////////////////////////////
    /// SOUND EFFECTS
    public void PlayButtonClickEffect(){
        PlaySoundEffect(0, 1);
    }

    public void PlayCoinEffect(){
        PlaySoundEffect(1, 1);
    }

    public void PlayPotionEffect(){
        int id = Random.Range(2,3);
        PlaySoundEffect(id, 0.6f);
    }

    public void PlayTeleportEffect(){
        PlaySoundEffect(4, 0.6f);
    }

    public void PlayDialogueEffect(){
        PlaySoundEffect(5, 0.6f);
    }
    public void PlayFinalEffect()
    {
        PlaySoundEffect(6,0.6f);
    }

    

    public void PlayMainMenuMusic(){
        PlayMusicTrack(0, 0.3f);
    }

    public void PlayGamePlayMusic(){
        PlayMusicTrack(1, 0.3f);
    }
}
