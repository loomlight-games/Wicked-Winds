using System.Collections;
using UnityEngine;
using System;
using UnityEditor;

/// <summary>
/// Public sound types enumeration
/// </summary>
public enum SoundType
{
    MenuMusic, GameplayMusic, ButtonClick, Coin, Water, Potion, Teleport,
    Dialogue, Cat, Bird, Owl, Final,
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
/// Handles music and sound effects reproduction. Requires two 
/// audioSource components 
/// </summary>
[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource effectsSource, musicSource;
    [SerializeField, Range(0, 1)] float fadeDuration = 0.5f;
    //[SerializeField, Range(0, 1)] float maxVolume = 1f;
    [SerializeField] SoundsList[] soundsList;

#if UNITY_EDITOR
    /// <summary>
    /// Automatically assigns the names of the sound types to the sound lists
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
        if (Instance == null)
        {
            Instance = this;

            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);// To avoid error in editor
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Retrieve all AudioSource components attached to the GameObject
        AudioSource[] audioSources = GetComponents<AudioSource>();

        // effectsSource is the first AudioSource component
        effectsSource = audioSources[0];

        // musicSource is the second AudioSource component
        musicSource = audioSources[1];
    }

    /// <summary>
    /// Plays random sound of a specific type. If it's music starts a transition
    /// </summary>
    public static void PlaySound(SoundType type, float volume = 1)
    {
        // Takes all the clips of the type
        AudioClip[] clips = Instance.soundsList[(int)type].sounds;

        // Randomly selects a clip from the list
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        // Type is music
        if (type == SoundType.MenuMusic || type == SoundType.GameplayMusic)
        {
            // Played in musicSource with a transition effect
            Instance.StartCoroutine(Instance.MusicTransition(randomClip, volume));
        }
        // Type is an effect
        else
            // Played in effectsSource
            Instance.effectsSource.PlayOneShot(randomClip, volume);
    }

    private IEnumerator MusicTransition(AudioClip newClip, float targetVolume)
    {
        // Fade out the current clip
        float startVolume = musicSource.volume;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Stop the current clip and change to the new clip
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in the new clip
        while (musicSource.volume < targetVolume)
        {
            musicSource.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Ensure the volume is set to the target volume at the end
        musicSource.volume = targetVolume;
    }
}

/// <summary>
/// To ensure that at least two AudioSources are attached to the SoundManager GO
/// </summary>
[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SoundManager soundManager = (SoundManager)target;

        // Check if there are exactly two AudioSource components
        AudioSource[] audioSources = soundManager.GetComponents<AudioSource>();
        if (audioSources.Length != 2)
        {
            EditorGUILayout.HelpBox("SoundManager requires exactly two AudioSource components.", MessageType.Warning);

            if (GUILayout.Button("Add Missing AudioSources"))
            {
                while (audioSources.Length < 2)
                {
                    soundManager.gameObject.AddComponent<AudioSource>();
                    audioSources = soundManager.GetComponents<AudioSource>();
                }
            }
        }
    }
}
