using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Work Out Poses", menuName = "Hold a Pose/Workout", order = 1)]
public class PoseWorkOut : ScriptableObject
{
    public int numberOfPoses;
    public RuntimeAnimatorController animatorController;
}
