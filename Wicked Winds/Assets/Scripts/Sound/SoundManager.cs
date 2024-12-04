using System.Collections;
using UnityEngine;

/// <summary>
/// Handles music and sound effects reproduction
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public float fadeDuration = 0.5f;
    public float maxVolume = 0.1f;

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
        audioSource.PlayOneShot(soundEffects[id], volume);
    }

    public void PlayMainMenuMusic()
    {
        PlayMusicTrack(0, maxVolume);
    }

    public void PlayGamePlayMusic()
    {
        PlayMusicTrack(1, maxVolume); 
    }

    private void PlayMusicTrack(int id, float maxVolume)
    {
        audioSource.Stop();
        audioSource.loop = true;
        // Luego, reproducimos la nueva m�sica con fade-in

        audioSource.clip = musicTracks[id];
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeInMusic());
    }

    // Music fade-in
    private IEnumerator FadeInMusic()
    {
        Debug.LogWarning("SUBIENDO EL VOLUMEN");
        

        // Fading in the music (gradualmente subiendo el volumen)
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, t / fadeDuration);  // Aumenta el volumen a targetVolume
            yield return null;
        }

        audioSource.volume = maxVolume;
    }

    //////////////////////////////////////////////////
    /// SOUND EFFECTS
    public void PlayButtonClickEffect()
    {
        PlaySoundEffect(0, 1);
    }

    public void PlayCoinEffect()
    {
        PlaySoundEffect(1, 1);
    }

    public void PlayPotionEffect()
    {
        int id = Random.Range(2, 3);
        PlaySoundEffect(id, 0.6f);
    }

    public void PlayTeleportEffect()
    {
        PlaySoundEffect(4, 0.6f);
    }

    public void PlayDialogueEffect()
    {
        PlaySoundEffect(5, 0.6f);
    }

    public void StopDialogueEffect()
    {
        if (audioSource.isPlaying && audioSource.clip == soundEffects[5]) // Verificar si el efecto de di�logo est� sonando
        {
            Debug.LogWarning("[SoundManager] Deteniendo el efecto de di�logo.");
            audioSource.Stop();
        }
    }

    public void PlayFinalEffect()
    {
        PlaySoundEffect(6, 0.6f);
    }
}
