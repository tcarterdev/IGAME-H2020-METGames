using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimedUI : MonoBehaviour
{
    private GameObject gameOverScreen;
    private GameObject timeToBeatScreen;

    private TextMeshProUGUI bronzeTimeText;
    private TextMeshProUGUI silverTimeText;
    private TextMeshProUGUI goldTimeText;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen = GameObject.Find("MatchCanvas").transform.Find("GameOverScreen").gameObject;
        timeToBeatScreen = gameOverScreen.transform.Find("TimeToBeatScreen").gameObject;

        bronzeTimeText = timeToBeatScreen.transform.Find("Bronze Time").GetComponent<TextMeshProUGUI>();
        silverTimeText = timeToBeatScreen.transform.Find("Silver Time").GetComponent<TextMeshProUGUI>();
        goldTimeText = timeToBeatScreen.transform.Find("Gold Time").GetComponent<TextMeshProUGUI>();
    }

    public void SetTimeText(float[] timesToBeat)
    {
        // Formats the timer to be 00:00.
        string bronzeMin = MathF.Floor(timesToBeat[0] / 60f).ToString("00");
        string bronzeSec = MathF.Floor(timesToBeat[0] % 60f).ToString("00");

        string silverMin = MathF.Floor(timesToBeat[1] / 60f).ToString("00");
        string silverSec = MathF.Floor(timesToBeat[1] % 60f).ToString("00");

        string goldMin = MathF.Floor(timesToBeat[2] / 60f).ToString("00");
        string goldSec = MathF.Floor(timesToBeat[2] % 60f).ToString("00");

        // Updates the text, and changes the colour of the colon to be iGame's pink/peach colour. 
        bronzeTimeText.text = string.Format("<sprite=0>{0}<color=#F95564>:</color>{1}", bronzeMin, bronzeSec);
        silverTimeText.text = string.Format("<sprite=1>{0}<color=#F95564>:</color>{1}", silverMin, silverSec);
        goldTimeText.text = string.Format("<sprite=2>{0}<color=#F95564>:</color>{1}", goldMin, goldSec);
    }
}
