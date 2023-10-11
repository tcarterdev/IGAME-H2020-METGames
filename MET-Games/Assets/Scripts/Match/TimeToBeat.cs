using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeToBeat : MonoBehaviour
{
    [SerializeField] private float bronzeTime;
    [SerializeField] private float silverTime;
    [SerializeField] private float goldTime;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => MatchGameManager.Instance.SetTimesToBeat(GetTimes()));
    }

    /// <summary>
    /// Returns the different times for bronze, silver and gold. 
    /// </summary>
    /// <returns>An array of floats storing the times.</returns>
    public float[] GetTimes()
    {
        float[] timesToBeat = new float[3];

        timesToBeat[0] = bronzeTime;
        timesToBeat[1] = silverTime;
        timesToBeat[2] = goldTime;

        return timesToBeat;
    }
}
