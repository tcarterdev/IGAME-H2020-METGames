using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Transform soundEffects;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("soundEffectVolume"))
        {
            PlayerPrefs.SetFloat("soundEffectVolume", 0.25f);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }

        CheckIfSoundIsMuted();

        PlayerPrefs.Save();
    }

    private void CheckIfSoundIsMuted()
    {
        if (!PlayerPrefs.HasKey("isSoundEffectMuted"))
        {
            PlayerPrefs.SetInt("isSoundEffectMuted", HelperFunctions.Instance.BoolToInt(false));

            soundEffects.Find("Mute").Find("ToggleOn").gameObject.SetActive(true);
            soundEffects.Find("Mute").Find("ToggleOff").gameObject.SetActive(false);

            MuteGame(false);
        }
        else
        {
            if (HelperFunctions.Instance.IntToBool(PlayerPrefs.GetInt("isSoundEffectMuted")))
            {
                soundEffects.Find("Mute").Find("ToggleOff").gameObject.SetActive(true);
                soundEffects.Find("Mute").Find("ToggleOn").gameObject.SetActive(false);

                MuteGame(true);
            }
            else
            {
                soundEffects.Find("Mute").Find("ToggleOn").gameObject.SetActive(true);
                soundEffects.Find("Mute").Find("ToggleOff").gameObject.SetActive(false);

                MuteGame(false);
            }
        }
    }

    public void LoadVolume()
    {
        // Update slider to match the saved value.
        soundEffectSlider.value = PlayerPrefs.GetFloat("soundEffectVolume");

        // Update volume to match the slider value.
        AudioListener.volume = soundEffectSlider.value;
    }

    public void UpdateVolume()
    {
        // Update volume to match the slider value.
        AudioListener.volume = soundEffectSlider.value;

        // Save the slider value as the sound effect value.
        PlayerPrefs.SetFloat("soundEffectVolume", soundEffectSlider.value);
        PlayerPrefs.Save();
    }

    public void TestVolume()
    {
        if (!soundEffectSlider.interactable) { return; }

        audioSource.Play();
    }

    public void MuteGame(bool state)
    {
        if (state)
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("isSoundEffectMuted", HelperFunctions.Instance.BoolToInt(state));

            soundEffectSlider.interactable = false;
        }
        else
        {
            AudioListener.volume = PlayerPrefs.GetFloat("soundEffectVolume");
            PlayerPrefs.SetInt("isSoundEffectMuted", HelperFunctions.Instance.BoolToInt(state));

            soundEffectSlider.interactable = true;
        }

        PlayerPrefs.Save();
    }
}
