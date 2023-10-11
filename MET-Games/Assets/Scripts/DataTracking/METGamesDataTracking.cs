using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class METGamesDataTracking : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);

    public static METGamesDataTracking _METGamesDataTracking;

    public MinigamesModes minigamesModes;
    public DataStucture DS_Timings;
    public DataStructure_FruitCatch DS_FruitCatch;
    public DataStructure_TrolleyDash DS_TrolleyDash;
    public DataStructure_HoldThePose DS_HoldThePose;
    public DataStructure_Match DS_Match;
    public DataStructure_SlidingPuzzle DS_SlidingPuzzle;
    public DataStructure_SnakesAndLadders DS_SnakesAndLadders;
    public DataStructure_TicTacToe DS_TicTacToe;

    DateTime _TimeStarted;
    DateTime _TimeEnded;
    public string JsonString;
    [System.Serializable]
    public class DataStucture
    {
        public GameStatus GameStatus;
        public string TimeStarted;
        public string TimeEnded;
        public float TimePlayed;
    }

    [System.Serializable]
    public class DataStructure_FruitCatch : DataStucture
    {
        public int Score;
        public int HighScore;
        public int FruitMissed;
        
    }
    [System.Serializable]
    public class DataStructure_TrolleyDash : DataStucture
    {
        public int WrongItems;
    }

    [System.Serializable]
    public class DataStructure_HoldThePose : DataStucture
    {
        public WorkOutLevels workoutLevel;
        public string WorkoutLevel;
  
    }

    public enum WorkOutLevels 
    { 
        LEVEL1,
        LEVEL2,
        LEVEL3,
        LEVEL4,
        LEVEL5,
        BREATHER
    }
    [System.Serializable]
    public class DataStructure_Match : DataStucture
    {
        public MatchMode Mode;
        public MatchGridSize GridSize;
        public int CorrectMatches;
        public int IncorrectMatches;
        public string Score;
    }

    public enum MatchMode
    {
        STANDARD,
        TIMED
    }

    [System.Serializable]
    public enum MatchGridSize
    {
        TWO,
        FOUR,
        SIX
    }

    [System.Serializable]
    public class DataStructure_SlidingPuzzle : DataStucture
    {
        public int TileMovedIntoCorrectSpot;
    
    }

    [System.Serializable]
    public enum MinigamesModes
    {
        FruitCatch,
        TrolleyDash,
        HoldThePose,
        Match,
        SlidingPuzzle,
        SnakesAndLadders,
        TicTacToe,
        Chess
    }

    public enum GameStatus 
    { 
        WON,
        LOST,
        EXITED,
        TIE,
        NA
    }

    [System.Serializable]
    public class DataStructure_SnakesAndLadders : DataStucture
    {
        public int numPlayers;

        public GameStatus PlayerOneGameStatus;
        public int PlayerOneTotalMoves;

        public GameStatus PlayerTwoGameStatus;
        public int PlayerTwoTotalMoves;

        public GameStatus PlayerThreeGameStatus;
        public int PlayerThreeTotalMoves;

        public GameStatus PlayerFourGameStatus;
        public int PlayerFourTotalMoves;
    }

    [System.Serializable]
    public class DataStructure_TicTacToe : DataStucture 
    {
        public GameStatus PlayerOneGameStatus;
        public GameStatus PlayerTwoGameStatus;
    }
    public void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(MimicGameplay());

    }

    // Update is called once per frame


    public void GetTimeStarted()
    {

        switch (minigamesModes)
        {
            case (MinigamesModes.FruitCatch):
                DS_Timings = DS_FruitCatch;
                break;

            case (MinigamesModes.Chess):

                break;

            case (MinigamesModes.HoldThePose):
                DS_Timings = DS_HoldThePose;
                break;

            case (MinigamesModes.Match):
                DS_Timings = DS_Match;
                break;

            case (MinigamesModes.SlidingPuzzle):
                DS_Timings = DS_SlidingPuzzle;
                break;

            case (MinigamesModes.SnakesAndLadders):
                DS_Timings = DS_SnakesAndLadders;
                break;

            case (MinigamesModes.TicTacToe):
                DS_Timings = DS_TicTacToe;
                break;

            case (MinigamesModes.TrolleyDash):
                DS_Timings = DS_TrolleyDash;
                break;

            default:
                break;
        }
        _TimeStarted = System.DateTime.Now;
        
        DS_Timings.TimeStarted = _TimeStarted.ToString();
    }

    public void GetTimeEnded()
    {
        _TimeEnded = System.DateTime.Now;
        DS_Timings.TimeEnded = _TimeEnded.ToString();
        float mins = (float)(_TimeEnded - _TimeStarted).TotalMinutes;
        mins = RoundMinutes(mins);
        DS_Timings.TimePlayed = mins;
    }

    #region FruitCatch

    public void FruitCatch_GameFinished(int Score, int HighScore, int FruitMissed, GameStatus gameStatus)
    {
        DS_FruitCatch.GameStatus = gameStatus;
        DS_FruitCatch.Score = Score;
        DS_FruitCatch.HighScore = HighScore;
        DS_FruitCatch.FruitMissed = FruitMissed;
        

    }
    #endregion

    #region TrolleyDash
    public void TrolleyDash_GameFinished(GameStatus gameStatus, int NumWrongItems) 
    {
        DS_TrolleyDash.GameStatus = gameStatus;
        DS_TrolleyDash.WrongItems = NumWrongItems;
    }
    #endregion

    #region Hold the pose
    public void HoldThePose_GameFinished(GameStatus gameStatus, string WorkoutLevel) 
    {
        DS_HoldThePose.GameStatus = gameStatus;

        WorkOutLevels convertedLevel;
        switch (WorkoutLevel) 
        {
            case ("L1"):
                convertedLevel = WorkOutLevels.LEVEL1;
                break;

            case ("L2"):
                convertedLevel = WorkOutLevels.LEVEL2;
                break;

            case ("L3"):
                convertedLevel = WorkOutLevels.LEVEL3;
                break;

            case ("L4"):
                convertedLevel = WorkOutLevels.LEVEL4;
                break;

            case ("L5"):
                convertedLevel = WorkOutLevels.LEVEL5;
                break;

            case("Breather"):
                convertedLevel = WorkOutLevels.BREATHER;
                break;

            default:
                convertedLevel = WorkOutLevels.LEVEL1;
                break;
        }

        DS_HoldThePose.WorkoutLevel = WorkoutLevel;
        DS_HoldThePose.workoutLevel = convertedLevel;
    }
    #endregion

    #region Match
    public void Match_GameFinished(GameStatus gameStatus, MatchMode Mode, string GridSize, string Score) 
    {
        DS_Match.GameStatus = gameStatus;
        DS_Match.Mode = Mode;
        DS_Match.Score = Score;
        
        //DS_Match.GridSize = GridSize;
    }
    public void Match_IncCorrectMatches() 
    {
        DS_Match.CorrectMatches++;
    }

    public void Match_IncInCorrectMatches()
    {
        DS_Match.IncorrectMatches++;
    }
    #endregion

    #region SlidingPuzzle
    public void SlidingPuzzle_GameFinsihed(GameStatus gameStatus) 
    {
        DS_SlidingPuzzle.GameStatus = gameStatus;
    }

    public void SlidingPuzzle_IncTileMovedToCorrectSpot() 
    {
        DS_SlidingPuzzle.TileMovedIntoCorrectSpot++;
    }





    #endregion

    #region SnakesAndLadders
    public void SnakesAndLadders_IncrementPlayerMove(int PlayerIndex) 
    {
        switch (PlayerIndex) 
        {
            case (0):
                DS_SnakesAndLadders.PlayerOneTotalMoves++;
                break;

            case (1):
                DS_SnakesAndLadders.PlayerTwoTotalMoves++;
                break;

            case (2):
                DS_SnakesAndLadders.PlayerThreeTotalMoves++;
                break;

            case (3):
                DS_SnakesAndLadders.PlayerFourTotalMoves++;
                break;
        }
    }

    public void SnakesAndLadders_SetNumOfPlayers(int num) 
    {
        DS_SnakesAndLadders.numPlayers = num;
    }

    public void SnakesAndLadders_PlayerWon(int PlayerIndex) 
    {
        switch (PlayerIndex)
        {
            case (0):
                DS_SnakesAndLadders.PlayerOneGameStatus = GameStatus.WON;
                DS_SnakesAndLadders.PlayerTwoGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerThreeGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerFourGameStatus = GameStatus.LOST;

                break;

            case (1):
                DS_SnakesAndLadders.PlayerOneGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerTwoGameStatus = GameStatus.WON;
                DS_SnakesAndLadders.PlayerThreeGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerFourGameStatus = GameStatus.LOST;

                break;

            case (2):
                DS_SnakesAndLadders.PlayerOneGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerTwoGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerThreeGameStatus = GameStatus.WON;
                DS_SnakesAndLadders.PlayerFourGameStatus = GameStatus.LOST;

                break;

            case (3):
                DS_SnakesAndLadders.PlayerOneGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerTwoGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerThreeGameStatus = GameStatus.LOST;
                DS_SnakesAndLadders.PlayerFourGameStatus = GameStatus.WON;

                break;
        }

        if (DS_SnakesAndLadders.numPlayers == 2) 
        {
            DS_SnakesAndLadders.PlayerThreeGameStatus = GameStatus.NA;
            DS_SnakesAndLadders.PlayerFourGameStatus = GameStatus.NA;
        }
        if (DS_SnakesAndLadders.numPlayers == 3)
        {
            DS_SnakesAndLadders.PlayerFourGameStatus = GameStatus.NA;
        }
    }

    public void SnakesAndLaddersExitedEarly() 
    {
        DS_Timings.GameStatus = GameStatus.EXITED;
        DS_SnakesAndLadders.PlayerOneGameStatus = GameStatus.EXITED;
        DS_SnakesAndLadders.PlayerTwoGameStatus = GameStatus.EXITED;
        DS_SnakesAndLadders.PlayerThreeGameStatus = GameStatus.EXITED;
        DS_SnakesAndLadders.PlayerFourGameStatus = GameStatus.EXITED;
    }
    #endregion

    #region TicTacToe

    public void TicTacToeComplete(int WinningPlayerIndex, bool Tied) 
    {
        if (!Tied)
        {
            if (WinningPlayerIndex == 0)
            {
                DS_TicTacToe.PlayerOneGameStatus = GameStatus.WON;
                DS_TicTacToe.PlayerTwoGameStatus = GameStatus.LOST;
            }
            else if (WinningPlayerIndex == 1)
            {
                DS_TicTacToe.PlayerOneGameStatus = GameStatus.LOST;
                DS_TicTacToe.PlayerTwoGameStatus = GameStatus.WON;
            }
        }
        else 
        {
            DS_TicTacToe.PlayerOneGameStatus = GameStatus.TIE;
            DS_TicTacToe.PlayerTwoGameStatus = GameStatus.TIE;
            DS_Timings.GameStatus = GameStatus.TIE;
        }


        GetTimeEnded();
        FinaliseData();
    }
    #endregion
    public void FinaliseData()
    {
        switch (minigamesModes)
        {
            case (MinigamesModes.FruitCatch):
                //DS_FruitCatch.Timings = DS_Timings;
                JsonString = JsonUtility.ToJson(DS_FruitCatch);
                SendData(JsonString);
                break;

            case (MinigamesModes.Chess):

                break;

            case (MinigamesModes.HoldThePose):
                //DS_HoldThePose.Timings = DS_Timings;
                JsonString = JsonUtility.ToJson(DS_HoldThePose);
                SendData(JsonString);
                break;

            case (MinigamesModes.Match):
                //DS_Match.Timings = DS_Timings;
                JsonString = JsonUtility.ToJson(DS_Match);
                SendData(JsonString);
                break;

            case (MinigamesModes.SlidingPuzzle):
                //DS_SlidingPuzzle.Timings = DS_Timings;
                JsonString = JsonUtility.ToJson(DS_SlidingPuzzle);
                SendData(JsonString);
                break;

            case (MinigamesModes.SnakesAndLadders):
                JsonString = JsonUtility.ToJson(DS_SnakesAndLadders);
                SendData(JsonString);
                break;

            case (MinigamesModes.TicTacToe):
                JsonString = JsonUtility.ToJson(DS_TicTacToe);
                SendData(JsonString);
                break;

            case (MinigamesModes.TrolleyDash):
                //DS_TrolleyDash.Timings = DS_Timings;
                JsonString = JsonUtility.ToJson(DS_TrolleyDash);
                SendData(JsonString);
                break;

            default:
                break;
        }
        //SendData(JsonString);
    }
    public IEnumerator MimicGameplay()
    {
        GetTimeStarted();
        yield return new WaitForSecondsRealtime(1f);
        
        switch (minigamesModes)
        {
            case (MinigamesModes.FruitCatch):
                int RandScore = UnityEngine.Random.Range(20, 150);
                int RandHighScore = UnityEngine.Random.Range(20, 150);
                int RandMissedFruit = UnityEngine.Random.Range(20, 150);
                bool WinLoss_FC = false;
                if (RandScore >= RandHighScore)
                {
                    WinLoss_FC = true;
                }
                //FruitCatch_GameFinished(RandScore, RandHighScore, RandMissedFruit, WinLoss_FC);
                break;

            case (MinigamesModes.Chess):

                break;

            case (MinigamesModes.HoldThePose):
                int RandWorkOutLevel = UnityEngine.Random.Range(1, 6);
                int RandWinLoss_HtP = UnityEngine.Random.Range(0, 2);
                bool WinLoss_HtP;
                if (RandWinLoss_HtP == 1)
                {
                    WinLoss_HtP = true;
                }
                else
                {
                    WinLoss_HtP = false;
                }
                //HoldThePose_GameFinished(WinLoss_HtP, "random");
                break;

            case (MinigamesModes.Match):
                int RandWinLoss_M = UnityEngine.Random.Range(0, 2);
                bool WinLoss_M;
                if (RandWinLoss_M == 1)
                {
                    WinLoss_M = true;
                }
                else
                {
                    WinLoss_M = false;
                }

                int RandMode_M = UnityEngine.Random.Range(0, 2);
                string Mode_M;
                if (RandMode_M == 1)
                {
                    Mode_M = "Standard";
                }
                else
                {
                    Mode_M = "Timed";
                }


                //Match_GameFinished(WinLoss_M,Mode_M, "4x4");

                Match_IncCorrectMatches();
                yield return new WaitForSecondsRealtime(1f);
                Match_IncInCorrectMatches();
                Match_IncCorrectMatches();
                yield return new WaitForSecondsRealtime(1f);
                Match_IncCorrectMatches();
                yield return new WaitForSecondsRealtime(1f);
                Match_IncCorrectMatches();
                Match_IncInCorrectMatches();
                yield return new WaitForSecondsRealtime(1f);
                Match_IncCorrectMatches();
                Match_IncInCorrectMatches();
                break;

            case (MinigamesModes.SlidingPuzzle):
                int RandWinLoss_SP = UnityEngine.Random.Range(0, 2);
                bool WinLoss_SP;
                if (RandWinLoss_SP == 1)
                {
                    WinLoss_SP = true;
                }
                else
                {
                    WinLoss_SP = false;
                }

                yield return new WaitForSecondsRealtime(1f);
                SlidingPuzzle_IncTileMovedToCorrectSpot();
                yield return new WaitForSecondsRealtime(1f);
                SlidingPuzzle_IncTileMovedToCorrectSpot();
                yield return new WaitForSecondsRealtime(1f);
                SlidingPuzzle_IncTileMovedToCorrectSpot();
                yield return new WaitForSecondsRealtime(1f);
                //SlidingPuzzle_GameFinsihed(WinLoss_SP);

                break;

            case (MinigamesModes.SnakesAndLadders):

                break;

            case (MinigamesModes.TicTacToe):

                break;

            case (MinigamesModes.TrolleyDash):
                int RandNumWrongItems = UnityEngine.Random.Range(20, 150);
                int RandWinLoss_TD = UnityEngine.Random.Range(0, 2);
                bool WinLoss_TD;
                if (RandWinLoss_TD == 1)
                {
                    WinLoss_TD = true;
                }
                else 
                {
                    WinLoss_TD = false;
                }
                //TrolleyDash_GameFinished(WinLoss_TD, RandNumWrongItems);
                break;

            default:
                break;
        }
        
        yield return new WaitForSecondsRealtime(1f);
        GetTimeEnded();
        FinaliseData();


    }

    public float RoundMinutes(float MinutesToRound)
    {
        float output = 0f;
        output = Mathf.Round(MinutesToRound * 1000f) / 1000f;
        return output;
    }

    public void SendData(string StringToSend) 
    {
        Debug.Log(StringToSend);
#if !UNITY_EDITOR
        
        //michael to add the send message code
        ShowMessage(StringToSend);
#endif

    }
}
