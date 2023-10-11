using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public bool running;
    public float timer;
    public float startTime;
    public TMP_Text timerText;
    [Space]
    public GameObject gameplay;
    public GameObject gameover;

    void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        timer = startTime;
        running = true;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerText.SetText(timer.ToString("F0") + " Seconds To Go!");

        if (timer <= 0)
        {
            TimerRunOut();
        }
    }

    private void TimerRunOut()
    {
        gameplay.SetActive(false);
        gameover.SetActive(true);
    }
}
