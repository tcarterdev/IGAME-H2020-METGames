using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyDashGameManager : MonoBehaviour
{

    public GameObject titleScreen;
    public GameObject howToPlay;
    public GameObject listGen;

    [Header("Sound")]
    [SerializeField] private SoundController soundController;

    public void HowToPlay()
    {
        titleScreen.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void ListGen()
    {
        howToPlay.SetActive(false);
        listGen.SetActive(true);
    }

    public void Start()
    {
        // Load volume.
        if (soundController != null) { soundController.LoadVolume(); }
    }
}
