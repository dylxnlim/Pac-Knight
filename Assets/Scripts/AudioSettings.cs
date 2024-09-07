using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadMusicPreferences();
        }
        else
        {
            AdjustMusicVolume();
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadSFXPreferences();
        }
        else
        {
            AdjustSFXVolume();
        }
    }

    public void AdjustMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    private void LoadMusicPreferences()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void AdjustSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    private void LoadSFXPreferences()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
