using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class PoseController : MonoBehaviour
{
    public Animator animator;
    public int poseIndex = 0;
    public PoseWorkOut poseWorkOut;
    [Space]
    [SerializeField] private float timer;
    [SerializeField] private float timeInEachPose;
    [Space]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text poseText;
    [Space]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject gameplayElements;

    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);

    private bool hasPoseStarted;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        hasPoseStarted = true;
        poseIndex = 0;
        MoveToNextPose();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            MoveToNextPose();
        }
        timerText.SetText(timer.ToString("F0") + " Seconds Left!");
        poseText.SetText("Pose " + poseIndex.ToString() + " / " + poseWorkOut.numberOfPoses.ToString());

        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.P))
            {
                MoveToNextPose();
            }
        #endif
    }

    private void MoveToNextPose()
    {
        if (poseIndex >= poseWorkOut.numberOfPoses)
        {
            EndWorkOut();
        }

        poseIndex += 1;
        animator.SetTrigger(poseIndex.ToString());

        timer = timeInEachPose;
    }

    private void EndWorkOut()
    {
    #if !UNITY_EDITOR
        // Send to JavaScript.
        //ShowMessage("Game Won");
    #endif

        winScreen.SetActive(true);

        gameplayElements.SetActive(false);
        gameplayUI.SetActive(false);

        hasPoseStarted = false;

        Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
        Camera.main.GetComponent<METGamesDataTracking>().HoldThePose_GameFinished(METGamesDataTracking.GameStatus.WON, poseWorkOut.name);
        Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
    }

    /// <summary>
    /// Returns if the game has been completed or not, by checking the active state of the win screen.
    /// </summary>
    /// <returns></returns>
    public bool GetHasGameCompleted() => winScreen.activeSelf;
    public bool GetHasPoseStarted() => hasPoseStarted;

    public void CallDataTracking_LeaveEarly()
    {
        Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
        Camera.main.GetComponent<METGamesDataTracking>().HoldThePose_GameFinished(METGamesDataTracking.GameStatus.EXITED, poseWorkOut.name);
        Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
    }
}
