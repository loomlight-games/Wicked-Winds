using System.Collections;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;

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
    [HideInInspector] public AudioSource effectsSource, musicSource;
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

        effectsSource.loop = false;
        musicSource.loop = true;
    }

    /// <summary>
    /// Plays random sound of a specific type
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
            // Stop the current clip and change to the new clip
            Instance.musicSource.Stop();
            Instance.musicSource.clip = randomClip;
            Instance.musicSource.volume = volume;
            Instance.musicSource.Play();

            // Played in musicSource with a transition
            //Instance.StartCoroutine(Instance.MusicTransition(randomClip, volume));
        }
        // Type is an effect
        else
            // Played in effectsSource
            Instance.effectsSource.PlayOneShot(randomClip, volume);
    }

    /// <summary>
    /// Plays random sound of a specific type
    /// </summary>
    public static void Play3DSound(SoundType type, Vector3 position, float volume = 1)
    {
        // Takes all the clips of the type
        AudioClip[] clips = Instance.soundsList[(int)type].sounds;

        // Randomly selects a clip from the list
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        // Plays the sound at the specified position
        AudioSource.PlayClipAtPoint(randomClip, position, volume);
    }

    public static void UpdateMusicVolume(float volume)
    {
        Instance.musicSource.volume = volume;
    }

    public static void UpdateEffectsVolume(float volume)
    {
        Instance.effectsSource.volume = volume;
    }

    // NOT WORKING /////////////////////////////////////////////////////////////
    private IEnumerator MusicTransition(AudioClip newClip, float targetVolume)
    {
        Debug.LogWarning("Transitioning to new music clip: " + newClip.name);

        float percent = 0;

        // Fade out the current clip
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            Instance.musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        // Stop the current clip and change to the new clip
        //musicSource.Stop();
        Instance.musicSource.clip = newClip;
        Instance.musicSource.Play();

        percent = 0;
        // Fade in the new clip
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            Instance.musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }

        // Ensure the volume is set to the target volume at the end
        Instance.musicSource.volume = targetVolume;
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
