using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject wincon;
    public GameObject titlescreen;
    public GameObject gameplay;
    public GameObject ListGen;

    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);

    public void SceneLoader(int buildIndex)
    {
       SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Reloads the active scene. 
    /// </summary>
    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void QuitGame()
    {
        /// Sliding Puzzle
        if (SceneManager.GetActiveScene().name == "Sliding Puzzle Final")
        {
            Debug.Log("[Sliding Puzzle] Quit Early!");

            // If the game wasn't completed, and we are quitting, count it as a loss.
            if (!Camera.main.GetComponent<GameScript>().GetHasGameCompleted())
            {
            #if !UNITY_EDITOR
                // Send to JavaScript.
                ShowMessage("Game Lost");
            #endif
            }
        }

        /// Trolley Dash
        if (SceneManager.GetActiveScene().name == "TrolleyDash")
        {
            Debug.Log("[Trolley Dash] Quit Early!");

            Trolley trolley = FindObjectOfType<Trolley>();

            // If the game wasn't completed, and we are quitting, count it as a loss.
            if (!trolley.GetHasGameCompleted())
            {
            #if !UNITY_EDITOR
                // Send to JavaScript.
                ShowMessage("Game Lost");
            #endif
            }

            Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();

            METGamesDataTracking.GameStatus gs;
            switch (trolley.GetWinLoss()) 
            {
                case (true):
                    gs = METGamesDataTracking.GameStatus.WON;
                    break;

                case (false):
                    gs = METGamesDataTracking.GameStatus.LOST;
                    break;
            }

            Camera.main.GetComponent<METGamesDataTracking>().TrolleyDash_GameFinished(METGamesDataTracking.GameStatus.EXITED, trolley.GetNumWrongItems());
            Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
        }

        /// Hold a Pose
        if (SceneManager.GetActiveScene().name == "HoldThePose")
        {
            Debug.Log("[Hold the Pose] Quit Early!");

            PoseHoldMenu poseHoldMenu = FindObjectOfType<PoseHoldMenu>();
            PoseController poseController = poseHoldMenu.GetPoseController();

            // If the game wasn't completed, and we are quitting, count it as a loss.
            if (!poseController.GetHasGameCompleted() && poseController.GetHasPoseStarted())
            {
            #if !UNITY_EDITOR
                // Send to JavaScript.
                ShowMessage("Game Lost");
            #endif
            }
        }

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void PlayAgainTrolleyDash()
    {
        wincon.SetActive(false);
        titlescreen.SetActive(true);
    }

    public void Gameplay()
    {
        ListGen.SetActive(false);
        gameplay.SetActive(true);
    }

    public void StartReloadSceneDelay()
    {
        StartCoroutine(DelayReloadScene());
    }

    private IEnumerator DelayReloadScene()
    {
        yield return new WaitForSeconds(0.1f);
        ReloadScene();
    }
}
