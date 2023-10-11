using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TakeABreath : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text subText;
    public float timer;

    public bool DataSent;
    public METGamesDataTracking _METGamesDataTracking;
    private void OnEnable()
    {
        DataSent = false;
        timer = 60f;
        Debug.Log("Timer Started");
        subText.SetText("Focus on your breath");
    
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.SetText(timer.ToString("F0") + " Seconds Remaining");
        }
        else
        {
            timerText.SetText("Good Job!");
            subText.SetText("");
            if (DataSent == false) 
            {
                DataSent = true;
                SendData();
            }
            
        }
    }

    public void SendData() 
    {
        _METGamesDataTracking.HoldThePose_GameFinished(METGamesDataTracking.GameStatus.WON, "Breather");
        _METGamesDataTracking.GetTimeEnded();
        _METGamesDataTracking.FinaliseData();
    }

    public void TakeABreathFinishedEarly() 
    {
        DataSent = false;
        _METGamesDataTracking.HoldThePose_GameFinished(METGamesDataTracking.GameStatus.LOST, "Breather");
        _METGamesDataTracking.GetTimeEnded();
        _METGamesDataTracking.FinaliseData();
    }
}
