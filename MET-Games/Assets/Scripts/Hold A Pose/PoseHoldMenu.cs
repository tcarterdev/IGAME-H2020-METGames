using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PoseHoldMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameplayElements;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject workouts;
    [SerializeField] private GameObject takeABreath;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private PoseController poseController;

    public void StartPoseMode(PoseWorkOut workOut)
    {
        //poseController.animator.runtimeAnimatorController = workOut.animatorController;
        poseController.poseWorkOut = workOut;
        poseController.poseIndex = 0;
        menu.SetActive(false);
        gameplayElements.SetActive(true);
        gameplayUI.SetActive(true);
        workouts.SetActive(false);
        takeABreath.SetActive(false);
    }

    public void BackToPoseMenu()
    {
        winScreen.SetActive(false);
        menu.SetActive(true);
        gameplayElements.SetActive(false);
        gameplayUI.SetActive(false);
        workouts.SetActive(false);
        takeABreath.SetActive(false);
    }

    public void OpenWorkouts()
    {
        workouts.SetActive(true);
        menu.SetActive(false);
        takeABreath.SetActive(false);
    }

    public void OpenTakeABreath()
    {
        menu.SetActive(false);
        gameplayElements.SetActive(false);
        gameplayUI.SetActive(false);
        workouts.SetActive(false);
        takeABreath.SetActive(true);
    }

    public PoseController GetPoseController() => poseController;
}
