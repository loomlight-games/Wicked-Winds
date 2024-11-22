using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinSound : MonoBehaviour
{

    [SerializeField] private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void reproduceSound()
    {
        if (soundManager != null)
            soundManager.SelectAudio(1, 0.6f);

    }
}
