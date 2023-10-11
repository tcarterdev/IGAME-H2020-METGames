using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
public class Trolley : MonoBehaviour, IDropHandler
{
   [SerializeField] public FruitEnum fenum;
    [SerializeField] ListGeneration listgen;
    public GameObject[] ticks;
    public int tickindex;
    public bool wincon;
    public GameObject winconobj;
    public GameObject gameplay;

    private bool hasGameCompleted;
    private int numOfWrongItem;

    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);

    // Start is called before the first frame update
    void Start()
    {
        if (listgen == null)
          {
           
          }

        hasGameCompleted = false;

    }
    // Update is called once per frame
    void Update()
    {
        if (wincon == true)
        {
        #if !UNITY_EDITOR
            // Send to JavaScript.
            ShowMessage("Game Won");
        #endif

            gameplay.SetActive(false);
            winconobj.SetActive(true);
        }
    }

    public void OnDrop(PointerEventData data)
    {
        
       
        if (listgen.RandomFoodList.Contains(fenum.type))
        {
            
            tickindex++;
        }
        else
        {
            numOfWrongItem++;
        }

        if(tickindex == 1)
        {
            ticks[1].SetActive(true);
        }

        if (tickindex == 2)
        {
            ticks[2].SetActive(true);
        }

        if (tickindex == 3)
        {
            ticks[3].SetActive(true);
        }

        if (tickindex == 4)
        {
            // If there is an AudioManager, play the ding sound effect. 
            if (AudioManager.Instance != null) { AudioManager.Instance.ding.Play(); }

            wincon = true;
            hasGameCompleted = true;
            ticks[3].SetActive(true);

            Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
            Camera.main.GetComponent<METGamesDataTracking>().TrolleyDash_GameFinished(METGamesDataTracking.GameStatus.WON, numOfWrongItem);
            Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
        }

    }

    public bool GetHasGameCompleted() => hasGameCompleted;

    public bool GetWinLoss() => wincon;

    public int GetNumWrongItems() => numOfWrongItem;
}
