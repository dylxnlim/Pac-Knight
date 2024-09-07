using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip bgm;
    public AudioClip buttonUI;
    public AudioClip startButtonUI;
    public AudioClip item;
    public AudioClip sword;
    public AudioClip death;
    public AudioClip coin;
    public AudioClip win;
    bool playing;

    private void Start()
    {
        for (int i = 0; i < Object.FindObjectsOfType<AudioManager>().Length; i++)
        {
            if(Object.FindObjectsOfType<AudioManager>()[i] != this)
            {
                if (Object.FindObjectsOfType<AudioManager>()[i].name == gameObject.name)
                {
                    Destroy(gameObject);
                }
            }
            
        }

        DontDestroyOnLoad(gameObject);
        musicSource.clip = bgm;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
