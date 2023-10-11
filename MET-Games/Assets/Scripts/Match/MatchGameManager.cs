using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class MatchGameManager : MonoBehaviour
{
    public static MatchGameManager Instance { get; private set; }

    [Tooltip("The number of cards required to match.")]
    [SerializeField] private int numberToMatch = 2;

    [SerializeField] private List<Icons> cardThemes;

    [SerializeField] private float scoreToGive = 5f;
    [SerializeField] private float incrementWaitTime = 0.1f;

    [SerializeField] private float cardFlipDelayTime = 0.15f;

    private float totalScore;
    private float FullScore;
    private List<GameObject> cards = new List<GameObject>();
    private List<int> cardIconCounters;
    private int cardFlipCounter;

    private int gridSize;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI scoreToAddText;
    private TextMeshProUGUI timerText;
    private float timer;
    private bool pauseTimer;
    private bool isTimedMode;
    
    private GameObject gameOverScreen;
    private GameObject timeToBeatScreen;

    private int randomThemeNumber;

    private bool isUiOpen;

    private float scoreOnCompletion;

    private bool gameWon;

    private GameObject menuButton;

    private TimeToBeat timeToBeat;
    private float[] timesToBeat;

    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one MatchGameManager! {transform} - {Instance}");
            Destroy(gameObject);
        }

        Instance = this;

        // Subscribe to OnGridCreated event.
        MatchGrid.Instance.OnGridCreated += MatchGrid_OnGridCreated;

        // Picks a random theme at the starts.
        randomThemeNumber = UnityEngine.Random.Range(0, cardThemes.Count);

        // Initialise counter list. 
        cardIconCounters = new List<int>();
        for (int i = 0; i < cardThemes[randomThemeNumber].icons.Count; i++)
        {
            cardIconCounters.Add(0);
        }

        timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        scoreToAddText = GameObject.Find("ScoreToAddText").GetComponent<TextMeshProUGUI>();

        gameOverScreen = GameObject.Find("MatchCanvas").transform.Find("GameOverScreen").gameObject;
        timeToBeatScreen = gameOverScreen.transform.Find("TimeToBeatScreen").gameObject;
        menuButton = GameObject.Find("MatchCanvas").transform.Find("Settings Button").gameObject;

        timeToBeat = FindObjectOfType<TimeToBeat>();
    }

    private void Start()
    {
        // Initialise and hide timer on game start. 
        pauseTimer = true;
        timerText.enabled = false;
        timerText.text = "0.00s";
        timer = 0f;

        scoreText.enabled = false;
        scoreToAddText.enabled = false;



        HandleSound();

        PlayerPrefs.Save();
    }

    private void HandleSound()
    {
        // If there is no soundEffectVolume key...
        if (!PlayerPrefs.HasKey("soundEffectVolume"))
        {
            // Set a default value.
            PlayerPrefs.SetFloat("soundEffectVolume", 0.25f);

            // Update volume to match the default volume.
            AudioListener.volume = PlayerPrefs.GetFloat("soundEffectVolume");
        }
        else
        {
            // If there is a key, update the volume to the saved value.
            AudioListener.volume = PlayerPrefs.GetFloat("soundEffectVolume");
        }

        // If there is no isSoundEffectMuted key...
        if (!PlayerPrefs.HasKey("isSoundEffectMuted"))
        {
            // Set a default value. 
            PlayerPrefs.SetInt("isSoundEffectMuted", HelperFunctions.Instance.BoolToInt(false));

            // Update volume to match the default volume.
            AudioListener.volume = PlayerPrefs.GetFloat("soundEffectVolume");
        }
        else
        {
            // Else if the sound is muted, update the volume to be zero.
            if (HelperFunctions.Instance.IntToBool(PlayerPrefs.GetInt("isSoundEffectMuted")))
            {
                AudioListener.volume = 0;
            }
            else
            {
                // Else, update the volume to match the saved value.
                AudioListener.volume = PlayerPrefs.GetFloat("soundEffectVolume");
            }
        }
    }

    private void Update()
    {
        // If the timer isn't paused, start counting up. 
        if (!pauseTimer)
        {
            timer += Time.deltaTime;

            // Formats the timer to be 00:00
            string minutes = MathF.Floor(timer / 60f).ToString("00");
            string seconds = MathF.Floor(timer % 60f).ToString("00");

            // Updates the text, and changes the colour of the colon to be iGame's pink/peach colour. 
            timerText.text = string.Format("{0}<color=#F95564>:</color>{1}", minutes, seconds);

        }

        // If the player has no score, set the text to show zero.
        if (totalScore == 0)
        {
            scoreText.text = "0";
        }
        else
        {
            // Else, update the text to show the players' score.
            scoreText.text = totalScore.ToString("#,#");

            // Update the comma in the formatted text to add iGame's pink colour.
            string formattedText = scoreText.text.Replace(",", "<color=#F95564>,</color>");

            // Update the permanent score text to show the changes. 
            scoreText.text = formattedText;

            // Update score text offset to display better.
            scoreText.GetComponent<RectTransform>().offsetMin = new Vector2(-28, scoreText.GetComponent<RectTransform>().offsetMin.y);
            scoreText.GetComponent<RectTransform>().offsetMax = new Vector2(28, scoreText.GetComponent<RectTransform>().offsetMax.y);

            if (gameWon) { scoreText.text = $"Total Score: {totalScore}"; }
        }
    }

    /// <summary>
    /// Called once the grid has been created. 
    /// </summary>
    /// <param name="sender">The object that invoked the event.</param>
    /// <param name="e">The event arguments. In this case is empty.</param>
    private void MatchGrid_OnGridCreated(object sender, EventArgs e)
    {
        gridSize = MatchGrid.Instance.GetGridSize();

        SetUpCards();

        if (isTimedMode)
        {
            timerText.enabled = true;
            scoreText.enabled = false;
            pauseTimer = false;
        }
        else
        {
            scoreText.enabled = true;
            scoreText.text = "0";
        }
    }

    /// <summary>
    /// Sets up the cards and gives them their icons. 
    /// </summary>
    private void SetUpCards()
    {
        for (int i = 0; i < MatchGrid.Instance.transform.childCount; i++)
        {
            MatchGrid.Instance.transform.GetChild(i).GetComponent<Card>().SetCardIcon(GetRandomIcon());
        }
    }

    /// <summary>
    /// Adds the flipped card to a list to be compared against. 
    /// </summary>
    /// <param name="card">The card that was flipped.</param>
    public void AddCardToList(GameObject card)
    {
        cards.Add(card);

        // If enough cards have been added to the list, check if they're all matching. 
        if (cards.Count == numberToMatch) { CheckCardsAreMatching(); }
    }

    /// <summary>
    /// Checks if the cards in the list are matching.
    /// </summary>
    private void CheckCardsAreMatching()
    {
        GameObject go = cards[0];
        for (int i = 0; i < cards.Count; i++)
        {
            // Skip to the next iteration if comparing against itself. 
            if (cards[i] == go) { continue; }

            // If the card icon matches the first in the list, hide from the game board. 
            if (cards[i].GetComponent<Card>().GetCardIcon() == go.GetComponent<Card>().GetCardIcon())
            {
                // Play sound on card match.
                if (AudioManager.Instance != null) { AudioManager.Instance.ding.Play(); }

                Camera.main.GetComponent<METGamesDataTracking>().Match_IncCorrectMatches();

                if (!isTimedMode)
                { 
                    // Give a score to the player.
                    AddToScore(scoreToGive);
                }

                // It's a match, remove/hide from the grid.
                foreach (GameObject card in cards)
                {
                    StartCoroutine(HideFromGrid(card));
                }
            }
            else
            {
                Camera.main.GetComponent<METGamesDataTracking>().Match_IncInCorrectMatches();

                foreach (GameObject card in cards)
                {
                    StartCoroutine(DelayCardFlip(card));
                }
            }
        }

        // Removes all the cards in the list, ready for the next go. 
        cards.Clear();
    }

    private IEnumerator DelayCardFlip(GameObject card)
    {
        yield return new WaitForSeconds(cardFlipDelayTime);

        card.GetComponent<Animator>().SetBool("isCardClicked", false);
    }

    /// <summary>
    /// Checks if there are cards still active. 
    /// 
    /// If there are, the game continues. If there aren't, the game is won. 
    /// </summary>
    public void CheckCardsAreActive()
    {
        int activeCards = 0;
        for (int i = 0; i < MatchGrid.Instance.transform.childCount; i++)
        {
            // If the object is active, increase the active counter. 
            if (MatchGrid.Instance.transform.GetChild(i).gameObject.activeInHierarchy) { activeCards++; }
        }

        // If there are no active cards, player has won the game. 
        if (activeCards == 0)
        {
            // If we are in Standard Mode, game won.
            if (!isTimedMode)
            {
                gameWon = true;

                Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
                Camera.main.GetComponent<METGamesDataTracking>().Match_GameFinished(METGamesDataTracking.GameStatus.WON, METGamesDataTracking.MatchMode.STANDARD, gridSize.ToString(), FullScore.ToString());
                Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();

#if !UNITY_EDITOR
                // Send to JavaScript.
                ShowMessage("Game Won");
#endif
            }

            // Pause timer. 
            pauseTimer = true;

            // Game end.
            gameOverScreen.SetActive(true);
            menuButton.SetActive(false);

            // Show the times to beat if the player is in the timed game mode. 
            if (isTimedMode) 
            { 
                timeToBeatScreen.SetActive(true);

                // Bronze
                if (timer > timesToBeat[0])
                {
                    // If we didn't beat the time, game lost.
                    Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
                    Camera.main.GetComponent<METGamesDataTracking>().Match_GameFinished(METGamesDataTracking.GameStatus.LOST, METGamesDataTracking.MatchMode.TIMED, gridSize.ToString(), timerText.text.ToString());
                    Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();

#if !UNITY_EDITOR
                    // Send to JavaScript.
                    ShowMessage("Game Lost");
#endif
                }
                else
                {
                    // If we did beat the time, game won.
                    Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();
                    Camera.main.GetComponent<METGamesDataTracking>().Match_GameFinished(METGamesDataTracking.GameStatus.WON, METGamesDataTracking.MatchMode.TIMED, gridSize.ToString(), timerText.text.ToString());
                    Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();

#if !UNITY_EDITOR
                    // Send to JavaScript.
                    ShowMessage("Game Won");
#endif
                }
            }
            else
            {
                // If we are not in timed move, move the score text above the play again buttons. 
                scoreText.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400f, -400f);

                // Add the score on completion to the total score. 
                AddToScore(scoreOnCompletion);
            }
        }
    }

    /// <summary>
    /// Hides the card from the grid (game board) after 0.25 seconds.
    /// 
    /// </summary>
    /// <param name="card">The flipped card that is to be hidden.</param>
    /// <returns></returns>
    private IEnumerator HideFromGrid(GameObject card)
    {
        // Resets the card flipped counter, so additional turns can be made. 
        ResetCardFlipCounter();

        // Wait 0.25 seconds before hiding the card. 
        yield return new WaitForSeconds(0.25f);
        card.SetActive(false);

        // Check if the game has been won or not. 
        CheckCardsAreActive();
    }
    
    /// <summary>
    /// Increases the card flip counter. 
    /// 
    /// Used to compare against when trying to flip more cards than intended. 
    /// </summary>
    public void IncreaseCardFlipCounter() => cardFlipCounter++;

    /// <summary>
    /// Resets the card flip counter back to zero, so additional turns can be made. 
    /// </summary>
    public void ResetCardFlipCounter() => cardFlipCounter = 0;

    /// <summary>
    /// Resets the card icon counters.
    /// </summary>
    public void ResetCardIconCounters()
    {
        for (int i = 0; i < cardIconCounters.Count; i++)
        {
            cardIconCounters[i] = 0;
        }
    }

    /// <summary>
    /// Resets the cards list. Prevents any left overs from previous games. 
    /// </summary>
    public void ResetCardsList() => cards.Clear();
    
    /// <summary>
    /// Resets the total score and re-enables the score text. 
    /// </summary>
    public void ResetScore()
    {
        totalScore = 0;

        scoreText.text = "0";
        scoreText.transform.parent.gameObject.SetActive(true);
    }

    /// <summary>
    /// Adds to the total score.
    /// </summary>
    /// <param name="scoreToAdd">the score to add.</param>
    public void AddToScore(float scoreToAdd)
    {
        scoreToAddText.enabled = true;
        scoreToAddText.text = $"<color=#F95564>+</color>{scoreToAdd}";
        scoreToAddText.GetComponent<Animator>().SetBool("isScoreAdded", true);

        StartCoroutine(IncrementScore(scoreToAdd));
    }

    /// <summary>
    /// Increments the score to add to the total, over a period of time.
    /// 
    /// Enables an animation to show the amount to add to the total.
    /// </summary>
    /// <param name="scoreToAdd">the score to add.</param>
    /// <returns></returns>
    private IEnumerator IncrementScore(float scoreToAdd)
    {
        FullScore += scoreToAdd;
        // Loop through the amount of score that needs adding, with a slight delay.
        for (int i = 0; i < scoreToAdd; i++)
        {
            yield return new WaitForSeconds(incrementWaitTime);
            totalScore++;
        }

        // Reset the animation, and hide the score to add text. 
        scoreToAddText.GetComponent<Animator>().SetBool("isScoreAdded", false);
        scoreToAddText.enabled = false;
    }

    public void CallDataTracking_LeaveEarly()
    {
        Camera.main.GetComponent<METGamesDataTracking>().GetTimeEnded();

        if (isTimedMode)
        {
            Camera.main.GetComponent<METGamesDataTracking>().Match_GameFinished(METGamesDataTracking.GameStatus.EXITED, METGamesDataTracking.MatchMode.TIMED, gridSize.ToString(), timerText.text.ToString());
        }
        else
        {
            Camera.main.GetComponent<METGamesDataTracking>().Match_GameFinished(METGamesDataTracking.GameStatus.EXITED, METGamesDataTracking.MatchMode.STANDARD, gridSize.ToString(), FullScore.ToString());
        }

        Camera.main.GetComponent<METGamesDataTracking>().FinaliseData();
    }


    #region Getters & Setters

    /// <summary>
    /// Gets the number of cards to match.
    /// </summary>
    /// <returns>An int of the number of cards required to match.</returns>
    public int GetNumberToMatch() => numberToMatch;

    /// <summary>
    /// Gets a random icon for the card. 
    /// </summary>
    /// <returns></returns>
    public Sprite GetRandomIcon()
    { 
        int cardIconMax = 0;
        if (MatchGrid.Instance.GetIsGridSquare())
        {
            // The limit of how many different card icons there can be.
            cardIconMax = gridSize / MatchGrid.Instance.GetGridX();
        }
        else
        {
            cardIconMax = gridSize / MatchGrid.Instance.GetGridY();
        }

        int randomNumber = 0;
        for (int i = 0; i < cardIconCounters.Count; i++)
        {
            // Depending on if the grid is grid, will depend on which value will be used. 
            if (MatchGrid.Instance.GetIsGridSquare())
            {
                randomNumber = UnityEngine.Random.Range(0, MatchGrid.Instance.GetGridX());
            }
            else
            {
                randomNumber = UnityEngine.Random.Range(0, MatchGrid.Instance.GetGridY());
            }
            
            // If the cardIcon has enough on the board, go back an iteration to find another.
            if (cardIconCounters[randomNumber] >= cardIconMax)
            {
                i--;
                continue;
            }
            else
            {
                // Increase the counter for the corresponding card icon.
                cardIconCounters[randomNumber]++;
                break;
            }
        }

        // Check if number has reached maximum 
        return cardThemes[randomThemeNumber].icons[randomNumber];
    }

    /// <summary>
    /// Gets the number of cards that have been flipped.
    /// </summary>
    /// <returns>An integer for the number of cards flipped in the turn.</returns>
    public int GetCardFlipCounter() => cardFlipCounter;

    /// <summary>
    /// Gets whether the UI is open or not. 
    /// </summary>
    /// <returns></returns>
    public bool GetIsUiOpen() => isUiOpen;

    /// <summary>
    /// Gets whether the mode is timed or not.
    /// </summary>
    /// <returns></returns>
    public bool GetIsTimedMode() => isTimedMode;

    /// <summary>
    /// Sets if the current game mode is timed. 
    /// </summary>
    /// <param name="state">the state to set the boolean to. True will show the timer, false will hide it.</param>
    public void SetIsTimedMode(bool state) => isTimedMode = state;

    /// <summary>
    /// Sets if the pause timer is paused or not.
    /// </summary>
    /// <param name="state">the state to set the boolean to. True will pause the timer, false will keep it running.</param>
    public void SetPauseTimer(bool state) => pauseTimer = state;

    /// <summary>
    /// Sets if the UI is open or not. 
    /// </summary>
    /// <param name="state">the state to set the boolean to. True will say the UI is open, false will say its not.</param>
    public void SetIsUiOpen(bool state) => isUiOpen = state;

    /// <summary>
    /// Sets the score on completion, depending on the grid size being completed.
    /// </summary>
    /// <param name="scoreOnCompletion">the amount of score to give the player upon completion of the grid.</param>
    public void SetScoreOnCompletion(float scoreOnCompletion) => this.scoreOnCompletion = scoreOnCompletion;

    public void SetTimesToBeat(float[] times) => timesToBeat = times;

#endregion
}
