using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; 
    [SerializeField] private AudioClip[] audios;

    private AudioSource controlAudio;

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
        controlAudio = GetComponent<AudioSource>();
    }



    public void SelectAudio(int indice, float volumen)
    {
        controlAudio.PlayOneShot (audios[indice], volumen);
    }

    // Metodo para detener el audio antes de tiempo
    public void StopAudio()
    {
        if (controlAudio.isPlaying) // Verifica si hay algo reproduciéndose
        {
            controlAudio.Stop();
        }
    }

}
