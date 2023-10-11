using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class SnakesLaddersManager : MonoBehaviour
{
    public event EventHandler OnTilesSetUp;

    [Header("Sound")]
    [SerializeField] private SoundController soundController;

    [Header("Gameboard")]
    [SerializeField] private List<SnakesLaddersTile> tiles;

    [Header("Player Data")]
    [SerializeField] private List<Player> players;
    [SerializeField]private Player currentPlayer;

    [Header("Gameplay")]
    [SerializeField] private Dice dice;
    [SerializeField] private Button diceButton;
    [SerializeField] private float tokenMoveSpeed;
    [SerializeField] private List<GameObject> playerTokens;
    [SerializeField] private List<Transform> movePoints;

    [Header("Gameboard for Menu")]
    [SerializeField] private GameObject gameboardObj;
    [SerializeField] private GameObject currentPlayerObj;
    [SerializeField] private GameObject settingsObj;
    [SerializeField] private Image diceImage;

    private int numOfPlayers;
    private bool hasPlayerWon;
    private bool hasTokenFinishedMoving;
    private bool isMovingOnLadderOrSnake;

    private List<int> playerPositions;

    private SnakesLaddersUIManager snakesLaddersUIManager;

    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);


    public METGamesDataTracking _MetGamesDataTracking;
    // Start is called before the first frame update
    void Start()
    {
        // Display the gameboard in the main menu.
        ShowGameboardAsMenu(true);

        // Load volume.
        if (soundController != null) { soundController.LoadVolume(); }

        // Get components.
        snakesLaddersUIManager = FindObjectOfType<SnakesLaddersUIManager>();
        dice = transform.Find("Gameboard").GetComponentInChildren<Dice>();

        // Initialise lists.
        playerPositions = new List<int>();
        movePoints = new List<Transform>();

        // Subscribe to dice roll event.
        dice.OnDiceRoll += Dice_OnDiceRoll;

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].number % 2 != 0)
            {
                // If the tile is an odd number, change the tile background to white, 
                // and the text colour to black.
                tiles[i].GetComponentInChildren<Image>().color = Color.white;
                tiles[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            }
        }
    }

    /// <summary>
    /// Shows the gameboard in the main menu.
    /// </summary>
    /// <param name="state"></param>
    public void ShowGameboardAsMenu(bool state)
    {
        currentPlayerObj.SetActive(!state);
        settingsObj.SetActive(!state);
        diceButton.interactable = !state;
        diceImage.enabled = !state;
    }

    /// <summary>
    /// Hides the player tokens, so they don't display when showing 
    /// the gameboard in the main menu.
    /// 
    /// Called from a button. 
    /// </summary>
    public void HidePlayerTokens()
    {
        for (int i = 0; i < playerTokens.Count; i++)
        {
            playerTokens[i].SetActive(false);
        }
    }

    /// <summary>
    /// Starts the game by setting up the players.
    /// 
    /// Called from a button.
    /// </summary>
    public void StartGame()
    {
        // Sets the current player to player #1. 
        currentPlayer = players[0];

        // Set the current player token as the last sibling, so it displays on top.
        playerTokens[currentPlayer.index].transform.SetAsLastSibling();

        for (int i = 0; i < numOfPlayers; i++)
        {
            // Update the player tokens to the corresponding players' colour. 
            playerTokens[i].GetComponent<Image>().color = players[i].playerColour;

            // Set the player positions to zero.
            playerPositions.Add(0);
        }
    }

    /// <summary>
    /// When the dice roll event is called, move the current players' token. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="diceRoll">The result of the dice roll.</param>
    private void Dice_OnDiceRoll(object sender, int diceRoll)
    {
        // Clear movePoints. 
        movePoints.Clear();

        // If the player doesn't get the exact roll needed, it skips their turn.
        if (playerPositions[currentPlayer.index] + diceRoll > 100) 
        {
            // Go to the next player.
            UpdateCurrentPlayer();

            // Re-enable the dice button.
            diceButton.interactable = true;
            
            // Return early.
            return;
        }

        // Add points for the token to move to.
        for (int i = 0; i <= diceRoll; i++)
        {
            for (int j = 0; j < tiles.Count; j++)
            {
                if (tiles[j].number == playerPositions[currentPlayer.index] + i)
                {
                    movePoints.Add(tiles[j].transform);
                }
            }
        }

        // Update player token position.
        playerPositions[currentPlayer.index] += diceRoll;

        // Update the players' token.
        StartCoroutine(MovePlayerTokenThroughPoints());
    }


    /// <summary>
    /// Moves the current players' token through each move point to reach its destination.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovePlayerTokenThroughPoints()
    {
        // Update the boolean to show the turn hasn't been finished yet. 
        hasTokenFinishedMoving = false;

        // Go through the movePoints and update the players' token to each one. 
        foreach (var movePoint in movePoints)
        {
            // Continue moving to the current tile until we have reached the threshold.
            while ((playerTokens[currentPlayer.index].transform.position - movePoint.position).sqrMagnitude > 0.001f)
            {
                // Move towards the next movePoint in the list.
                playerTokens[currentPlayer.index].transform.position = Vector3.MoveTowards(playerTokens[currentPlayer.index].transform.position, movePoint.position, tokenMoveSpeed * Time.deltaTime);
                yield return null;
            }

            // Adds a small delay between each tile movement.
            yield return new WaitForSeconds(0.025f);
        }

        // Check the type of tile the current player has landed on.
        CheckTile();

        // Check if player has won.
        for (int i = 0; i < numOfPlayers; i++)
        {
            // If a player has reached tile 100, they have won.
            if (playerPositions[i] == 100)
            {
                // Update the boolean to show a player has won.
                hasPlayerWon = true;
                
                // If there is an AudioManager, play the ding sound effect. 
                if (AudioManager.Instance != null) { AudioManager.Instance.ding.Play(); }

                if (i == 0)
                {
                    
                    // If the player that won is player #1, call the iGame event for a Game Won. 
                    if (currentPlayer.index == players[0].index)
                    {
                    #if !UNITY_EDITOR
                        // Send to JavaScript.
                        ShowMessage("Game Won");
                    #endif
                    }
                    else // If it wasn't player #1 that won, call the iGame event for a Game Lost. 
                    {
                    #if !UNITY_EDITOR
                        // Send to JavaScript.
                        ShowMessage("Game Lost");
                    #endif
                    }
                }

                // Player has won.
                
                snakesLaddersUIManager.ShowWinMessage();
            }
        }

        // If the player token has reached its destination, update the boolean to show the turn has finished.
        if ((playerTokens[currentPlayer.index].transform.position - movePoints[movePoints.Count - 1].position).sqrMagnitude < 0.001f)
        {
            hasTokenFinishedMoving = true;
        }

        // Update the current player to the next player in line. 
        UpdateCurrentPlayer();
    }

    /// <summary>
    /// Checks the tile the player has landed on,
    /// and handles the effects of the tile. 
    /// </summary>
    private void CheckTile()
    {
        // Check current tile. 
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].number == playerPositions[currentPlayer.index])
            {
                switch (tiles[i].GetTileLinkType())
                {
                    case SnakesLaddersTile.LinkType.Ladder: // Increases the player token position.

                        // Move token to the new position.
                        StartCoroutine(MovePlayerToken(tiles[i].GetTargetLinkTile().transform.position));

                        // Update current position.
                        playerPositions[currentPlayer.index] = tiles[i].GetTargetLinkTile().number;

                        break;

                    case SnakesLaddersTile.LinkType.Snake:  // Decreases the player token position. 

                        // Move token to the new position.
                        StartCoroutine(MovePlayerToken(tiles[i].GetTargetLinkTile().transform.position));

                        // Update current position.
                        playerPositions[currentPlayer.index] = tiles[i].GetTargetLinkTile().number;

                        break;

                    case SnakesLaddersTile.LinkType.None:   // Does nothing. 
                        return;
                }
            }
        }
    }

    /// <summary>
    /// Moves the current players' token to the end position (destination).
    /// </summary>
    /// <param name="endPos">The destination for the player token.</param>
    /// <returns></returns>
    private IEnumerator MovePlayerToken(Vector3 endPos)
    {
        hasTokenFinishedMoving = false;

        // Continue moving to the current tile until we have reached the threshold.
        while ((playerTokens[currentPlayer.index].transform.position - endPos).sqrMagnitude > 0.001f)
        {
            // Move towards the next movePoint in the list.
            playerTokens[currentPlayer.index].transform.position = Vector3.MoveTowards(playerTokens[currentPlayer.index].transform.position, endPos, tokenMoveSpeed * Time.deltaTime);
            yield return null;
        }

        // If the player token has reached its destination, update the boolean to show the turn has finished.
        if ((playerTokens[currentPlayer.index].transform.position - endPos).sqrMagnitude < 0.001f)
        {
            hasTokenFinishedMoving = true;
        }

        // Update the current player to the next player in line. 
        UpdateCurrentPlayer();
    }

    /// <summary>
    /// Updates the current player to the next player in line.
    /// </summary>
    private void UpdateCurrentPlayer()
    {
        // Return early if the player token hasn't finished moving.
        if (!hasTokenFinishedMoving) { return; }

        // Re-enable dice button.
        diceButton.interactable = true;
        _MetGamesDataTracking.SnakesAndLadders_IncrementPlayerMove(currentPlayer.index);
        if (numOfPlayers == 2)
        {
            if (currentPlayer.index + 1 > 1)    // If we go out of bounds, go back to the beginning. 
            {
                // Set player 1 as the current player.
                currentPlayer = players[0];
            }
            else
            {
                // Set player 2 as the current player.
                currentPlayer = players[currentPlayer.index + 1];
            }
        }
        else if (numOfPlayers == 3)
        {
            if (currentPlayer.index + 1 > 2)    // If we go out of bounds, go back to the beginning. 
            {
                // Set player 1 as the current player.
                currentPlayer = players[0];
            }
            else if (currentPlayer.index < 2)   // If we are within bounds, increase the index and update the current player.
            {
                currentPlayer = players[currentPlayer.index + 1];
            }
        }
        else if (numOfPlayers == 4)
        {
            if (currentPlayer.index + 1 > 3)    // If we go out of bounds, go back to the beginning. 
            {
                // Set player 1 as the current player.
                currentPlayer = players[0];
            }
            else if (currentPlayer.index < 3)   // If we are within bounds, increase the index and update the current player.
            {
                currentPlayer = players[currentPlayer.index + 1];
            }
        }

        
        // Set the current player as the last sibling so it appears on top.
        playerTokens[currentPlayer.index].transform.SetAsLastSibling();
    }

    #region Getters & Setters

    /// <summary>
    /// Gets the tiles that make up the gameboard.
    /// </summary>
    /// <returns>A list of tiles.</returns>
    public List<SnakesLaddersTile> GetTiles() => tiles;

    /// <summary>
    /// Sets the number of players.
    /// 
    /// Called from a button.
    /// </summary>
    /// <param name="numOfPlayers">The decided number of players.</param>
    public void SetNumOfPlayers(int numOfPlayers)
    {
        _MetGamesDataTracking.SnakesAndLadders_SetNumOfPlayers(numOfPlayers);
        // Update the number of players to the decided number of players.
        this.numOfPlayers = numOfPlayers;

        for (int i = 0; i < numOfPlayers; i++)
        {
            // Enable enough player tokens to match the decided number of players.
            playerTokens[i].SetActive(true);

            // Enable enough player customisation options to match the decided number of players.
            snakesLaddersUIManager.GetPlayerNames_IF()[i].transform.parent.gameObject.SetActive(true);
        }

        // If the number of players is 2, disable player #3 and player #4 tokens and customisation options.
        if (numOfPlayers == 2)
        {
            playerTokens[2].SetActive(false);
            snakesLaddersUIManager.GetPlayerNames_IF()[2].transform.parent.gameObject.SetActive(false);

            playerTokens[3].SetActive(false);
            snakesLaddersUIManager.GetPlayerNames_IF()[3].transform.parent.gameObject.SetActive(false);
        }
        else if (numOfPlayers == 3) // If the number of players is 3, disable player #4 token and customisation options.
        {
            playerTokens[3].SetActive(false);
            snakesLaddersUIManager.GetPlayerNames_IF()[3].transform.parent.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Gets the number of players. 
    /// </summary>
    /// <returns></returns>
    public int GetNumOfPlayers() => numOfPlayers;

    /// <summary>
    /// Sets the player name for the given index. 
    /// </summary>
    /// <param name="playerIndex">The index of the player to update.</param>
    /// <param name="playerName">The name to update.</param>
    public void SetPlayerName(int playerIndex, string playerName)
    {
        // Update the player name at the given index. 
        players[playerIndex].playerName = playerName;
    }

    /// <summary>
    /// Sets the player colour for the given index.
    /// </summary>
    /// <param name="playerIndex">The index of the player to update.</param>
    /// <param name="colour">The colour to update.</param>
    public void SetPlayerColour(int playerIndex, Color colour)
    {
        // Update the player colour at the given index.
        players[playerIndex].playerColour = colour;
    }

    /// <summary>
    /// Gets the player at the given index.
    /// </summary>
    /// <param name="playerIndex">The index of the player to get.</param>
    /// <returns></returns>
    public Player GetPlayer(int playerIndex) => players[playerIndex];

    /// <summary>
    /// Gets the hasPlayerWon boolean.
    /// </summary>
    /// <returns></returns>
    public bool GetHasPlayerWon() => hasPlayerWon;

    /// <summary>
    /// Gets the current player.
    /// </summary>
    /// <returns></returns>
    public Player GetCurrentPlayer() => currentPlayer;

    /// <summary>
    /// Gets the token of the current player.
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentPlayerToken() => playerTokens[currentPlayer.index];

    #endregion

    private void OnDisable()
    {
        // Reset the player names and colours back to their defaults.
        for (int i = 0; i < players.Count; i++)
        {
            players[i].playerName = $"Player #{i + 1}";
            players[i].playerColour = players[i].defaultColour;
        }
    }

    public void LeftEarly() 
    {
        _MetGamesDataTracking.GetTimeEnded();
        _MetGamesDataTracking.SnakesAndLaddersExitedEarly();
        _MetGamesDataTracking.FinaliseData();

    }
}
