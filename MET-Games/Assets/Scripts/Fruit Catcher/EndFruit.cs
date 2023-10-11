using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class EndFruit : MonoBehaviour
{
    public FruitLauncher launcher;
    public TMP_Text fruitCaught;
    public TMP_Text fruitMissed;
    public TMP_Text highScore;
    public GameObject gameplay;
    public GameObject endcreen;
    public Timer timer;

    private bool gameWon;

    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);

    private void OnEnable()
    {
        Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();

        if (launcher.fruitCaught >= PlayerPrefs.GetInt("MostFruitCaught"))
        {
            PlayerPrefs.SetInt("MostFruitCaught", launcher.fruitCaught);

#if !UNITY_EDITOR
            // Send to JavaScript.
            ShowMessage("Game Won");
#endif

            gameWon = true;
        }
        else
        {
#if !UNITY_EDITOR
            // Send to JavaScript.
            ShowMessage("Game Lost");
#endif

            gameWon = false;
        }


        //Time.timeScale = 0f;
        fruitCaught.SetText("Fruit Caught: " + launcher.fruitCaught.ToString());
        fruitMissed.SetText("Fruit Missed: " + launcher.fruitMissed.ToString());
        highScore.SetText("High Score: " + PlayerPrefs.GetInt("MostFruitCaught").ToString());


        METGamesDataTracking.GameStatus gameStatus;
        switch (gameWon) 
        {
            case (true):
                gameStatus = METGamesDataTracking.GameStatus.WON;
                break;
            case (false):
                gameStatus = METGamesDataTracking.GameStatus.LOST;
                break;
        }
        Camera.main.GetComponent<METGamesDataTracking>().FruitCatch_GameFinished(launcher.fruitCaught, PlayerPrefs.GetInt("MostFruitCaught"), launcher.fruitMissed, gameStatus);
        Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
    }

    public void ReloadGame()
    {
        launcher.fruitMissed = 0;
        launcher.fruitCaught = 0;
        gameplay.SetActive(true);
        gameplay.SetActive(false);
        timer.StartTimer();
    }
}
