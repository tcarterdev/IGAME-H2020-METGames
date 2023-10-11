using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource buttonClick;
    public AudioSource ding;
    public AudioSource diceRoll;
    public AudioSource diceRollFinished;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one AudioManager! {transform} - {Instance}");
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void PlayButtonSound() => buttonClick.Play();
}
