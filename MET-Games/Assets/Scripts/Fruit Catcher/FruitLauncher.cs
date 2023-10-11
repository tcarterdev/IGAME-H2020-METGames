using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.WSA;

public class FruitLauncher : MonoBehaviour
{
    public int fruitCaught;
    public int fruitMissed;
    [Space]
    public float spawnTime;
    [Space]
    public GameObject[] fruitPrefabs;
    [SerializeField] private Transform fruitSpawnTransform;
    [Space]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private GameObject endScreen;

    [Tooltip("It's attached to the SoundEffectSlider in Settings Menu")]
    [SerializeField] private SoundController soundController;

    private void Start()
    {
        // Load volume.
        if (soundController != null) { soundController.LoadVolume(); }
    }

    private void LaunchNewFruit()
    {
        int ranNum = Random.Range(0, 5);
        GameObject fruit = Instantiate(fruitPrefabs[ranNum]);
        fruit.transform.SetParent(this.transform);
        fruit.transform.position = fruitSpawnTransform.position;
    }

    public void StartNewRound()
    {
        mainMenu.SetActive(false);
        gameplay.SetActive(true);
        endScreen.SetActive(false);
        InvokeRepeating("LaunchNewFruit", spawnTime, spawnTime);
    }

    public void CallDataTracking_LeaveEarly()
    {
        Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
        Camera.main.GetComponent<METGamesDataTracking>().FruitCatch_GameFinished(fruitCaught, PlayerPrefs.GetInt("MostFruitCaught"), fruitMissed, METGamesDataTracking.GameStatus.EXITED);
        Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
    }

}
