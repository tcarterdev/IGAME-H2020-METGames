using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [SerializeField] private Transform emptySpace = null;
    private Camera mainCamera;
    [SerializeField] Tiles[] tiles;
    private int emptySpaceIndex = 15;
    public GameObject completeButton;

    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);

    private bool hasGameCompleted;
    private bool DataSent;
    void Start()
    {
        mainCamera = Camera.main;
        Shuffle();
        Camera.main.GetComponent<METGamesDataTracking>().GetTimeStarted();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit)
            {
                if (Vector2.Distance(emptySpace.position, hit.transform.position) < 2)
                {
                    Vector2 lastEmptySpacePosition = emptySpace.position;
                    Tiles thisTile = hit.transform.GetComponent<Tiles>();
                    emptySpace.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptySpacePosition;
                    int tileIndex = findIndex(thisTile);
                    tiles[emptySpaceIndex] = tiles[tileIndex];
                    tiles[tileIndex] = null;
                    emptySpaceIndex = tileIndex;

                    if (thisTile.targetPosition == thisTile.GetCorrectPos())
                    {
                        Camera.main.GetComponent<METGamesDataTracking>().SlidingPuzzle_IncTileMovedToCorrectSpot();
                    }
                }
            }
        }

        int correctTiles = 0;
        foreach (var a in tiles)
        {
            if (a != null)
            {
                if (a.inRightPlace)
                    correctTiles++;
            }
        }

        if (correctTiles == tiles.Length - 1)
        {
#if !UNITY_EDITOR
            // Send to JavaScript.
            ShowMessage("Game Won");
#endif
            if (!DataSent) 
            {
                Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
                Camera.main.GetComponent<METGamesDataTracking>().SlidingPuzzle_GameFinsihed(METGamesDataTracking.GameStatus.WON);
                Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
                DataSent = true;
            }
            

            Debug.Log("U Win");
            completeButton.SetActive(true);
            hasGameCompleted = true;
        }
    }
    //Shuffle Method
    public void Shuffle()
    {

        if (emptySpaceIndex != 15)
        {
            var tileOn15LastPos = tiles[15].targetPosition;
            tiles[15].targetPosition = emptySpace.position;
            emptySpace.position = tileOn15LastPos;
            tiles[emptySpaceIndex] = tiles[15];
            tiles[15] = null;
            emptySpaceIndex = 15;
        }
        int invertion;
        do
        {
            for (int i = 0; i <= 14; i++)
            {
                var lastPos = tiles[i].targetPosition;
                int randomIndex = Random.Range(0, 14);
                tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition = lastPos;

                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;

                if (tiles[i].targetPosition == tiles[i].GetCorrectPos())
                {
                    Camera.main.GetComponent<METGamesDataTracking>().SlidingPuzzle_IncTileMovedToCorrectSpot();
                }

            }
            invertion = GetInversions();
        } while (invertion%2 != 0);
        
    }

    public int findIndex(Tiles ts)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] == ts)
                {
                    return i;
                }

            }
        }

        return -1;
    }

    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }

    public bool GetHasGameCompleted() => hasGameCompleted;

    public void CallDataTracking_LeaveEarly()
    {
        Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
        Camera.main.GetComponent<METGamesDataTracking>().SlidingPuzzle_GameFinsihed(METGamesDataTracking.GameStatus.EXITED);
        Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
    }

}

    
