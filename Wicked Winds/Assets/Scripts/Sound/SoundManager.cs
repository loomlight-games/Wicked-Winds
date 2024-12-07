using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Public sound types enumeration
/// </summary>
public enum SoundType
{
    MenuMusic,
    GameplayMusic,
    ButtonClick,
    Coin,
    Water,
    Potion,
    Teleport,
    Dialogue,
    Cat,
    Bird,
    Owl,
    Final,
}


/// <summary>
/// Allows to handle multiple sounds of the same type
/// </summary>
[Serializable]
public struct SoundsList
{
    [HideInInspector] public string name;
    [SerializeField] public AudioClip[] sounds;
}

/// <summary>
/// Handles music and sound effects reproduction
/// </summary>
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource effectsSource, musicSource;
    [SerializeField, Range(0, 1)] float fadeDuration = 0.5f;
    [SerializeField, Range(0, 1)] float maxVolume = 0.1f;
    [SerializeField] SoundsList[] soundsList;

#if UNITY_EDITOR
    /// <summary>
    /// To automatically call each sound list by a type in the editor
    /// </summary>
    void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundsList, names.Length);

        for (int i = 0; i < soundsList.Length; i++)
            soundsList[i].name = names[i];
    }
#endif

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {

    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = Instance.soundsList[(int)sound].sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance.effectsSource.PlayOneShot(randomClip, volume);
    }

    // Music fade-in
    private IEnumerator FadeInMusic()
    {
        Debug.LogWarning("SUBIENDO EL VOLUMEN");


        // Fading in the music (gradualmente subiendo el volumen)
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            effectsSource.volume = Mathf.Lerp(0, maxVolume, t / fadeDuration);  // Aumenta el volumen a targetVolume
            yield return null;
        }

        effectsSource.volume = maxVolume;
    }
}
