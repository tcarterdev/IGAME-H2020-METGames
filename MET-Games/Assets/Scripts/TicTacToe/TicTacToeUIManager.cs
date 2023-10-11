using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TicTacToeUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentPlayerName;

    [Header("Player Names")]
    [SerializeField] private TMP_InputField playerOne_IF;
    [SerializeField] private TMP_InputField playerTwo_IF;

    // Across Mode
    [SerializeField] private TextMeshProUGUI playerOneText;
    [SerializeField] private TextMeshProUGUI playerTwoText;

    private TicTacToeManager ticTacToeManager;

    [Header("Player Name Objects")]
    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;

    private PlayerColour playerColour;

    private void OnEnable()
    {
        ticTacToeManager = FindObjectOfType<TicTacToeManager>();
        playerColour = FindObjectOfType<PlayerColour>();

        // Return early if it isn't Across play style.
        if (!ticTacToeManager.GetIsAcrossMode()) { return; }

        currentPlayerName.text = ticTacToeManager.GetCurrentPlayer().playerName;

        if (ticTacToeManager.GetIsPlayerOne())
        {
            playerOneText.enabled = true;
            playerTwoText.enabled = false;
        }
        else
        {
            playerTwoText.enabled = true;
            playerOneText.enabled = false;
        }
    }

    private void Update()
    {
        // If the game hasn't been won yet, and the turn counter hasn't reached the max, update the UI.
        if (ticTacToeManager.GetHasGameStarted() && !ticTacToeManager.GetHasPlayerWon() && ticTacToeManager.GetTurnCounter() != 9)
        {
            if (!ticTacToeManager.GetIsAcrossMode())
            {
                // If we are in SideBySide, only update the currentPlayerName text.
                currentPlayerName.text = ticTacToeManager.GetCurrentPlayer().playerName;
                currentPlayerName.color = ticTacToeManager.GetCurrentPlayer().playerColour;
            }
            else
            {
                if (ticTacToeManager.GetIsPlayerOne())
                {
                    playerOneText.enabled = true;
                    playerTwoText.enabled = false;

                    playerOneText.text = ticTacToeManager.GetCurrentPlayer().playerName;
                    playerOneText.color = ticTacToeManager.GetCurrentPlayer().playerColour;
                }
                else
                {
                    playerTwoText.enabled = true;
                    playerOneText.enabled = false;

                    playerTwoText.text = ticTacToeManager.GetCurrentPlayer().playerName;
                    playerTwoText.color = ticTacToeManager.GetCurrentPlayer().playerColour;
                }
            }
        }
    }

    #region Messages

    /// <summary>
    /// Shows the tie message depending on the play mode style.
    /// </summary>
    public void ShowTieMessage()
    {
        if (ticTacToeManager.GetIsAcrossMode())
        {
            // Update playerOneText to show game result.
            playerOneText.enabled = true;
            playerOneText.text = $"It's a tie!";
            playerOneText.color = ticTacToeManager.GetPlayerOne().playerColour;

            // Update playerTwoText to show game result.
            playerTwoText.enabled = true;
            playerTwoText.text = $"It's a tie!";
            playerTwoText.color = ticTacToeManager.GetPlayerTwo().playerColour;
        }
        else
        {
            // Only update the currentPlayerName text to show game result.
            currentPlayerName.text = "It's a tie!";
            currentPlayerName.color = playerColour.GetIGameColour();
        }
    }

    /// <summary>
    /// Shows the win message depending on the play mode style. 
    /// Updates the text based on the player that won.
    /// </summary>
    /// <param name="playerIndex">the index of the player that won.</param>
    public void ShowWinMessage(int playerIndex)
    {
        if (ticTacToeManager.GetIsAcrossMode())
        {
            if (playerIndex == 1)
            {
                // Update both players' text to show player 1 won.
                playerOneText.enabled = true;
                playerOneText.text = $"{ticTacToeManager.GetPlayer(1).playerName} Wins!";
                playerOneText.color = ticTacToeManager.GetPlayer(1).playerColour;

                playerTwoText.enabled = true;
                playerTwoText.text = $"{ticTacToeManager.GetPlayer(1).playerName} Wins!";
                playerTwoText.color = ticTacToeManager.GetPlayer(1).playerColour;
            }
            else
            {
                // Update both players' text to show player 2 won.
                playerOneText.enabled = true;
                playerOneText.text = $"{ticTacToeManager.GetPlayer(2).playerName} Wins!";
                playerOneText.color = ticTacToeManager.GetPlayer(2).playerColour;

                playerTwoText.enabled = true;
                playerTwoText.text = $"{ticTacToeManager.GetPlayer(2).playerName} Wins!";
                playerTwoText.color = ticTacToeManager.GetPlayer(2).playerColour;
            }
        }
        else
        {
            if (playerIndex == 1)
            {
                // Update the text to show win state. 
                currentPlayerName.text = $"{ticTacToeManager.GetPlayer(1).playerName} Wins!";
                currentPlayerName.color = ticTacToeManager.GetPlayer(1).playerColour;
            }
            else
            {
                // Update the text to show win state. 
                currentPlayerName.text = $"{ticTacToeManager.GetPlayer(2).playerName} Wins!";
                currentPlayerName.color = ticTacToeManager.GetPlayer(2).playerColour;
            }
        }
    }

    #endregion

    #region Getters & Setters

    /// <summary>
    /// Gets the player text object.
    /// </summary>
    /// <param name="num">the player index.</param>
    /// <returns></returns>
    public TextMeshProUGUI GetPlayerText(int num)
    {
        if (num == 1) 
        { 
            return playerOneText; 
        }
        else if (num == 2) 
        { 
            return playerTwoText; 
        }

        return null;
    }

    /// <summary>
    /// Sets both player names using the input fields.
    /// 
    /// Called by a button. 
    /// </summary>
    public void SetPlayerNames()
    {
        if (playerOne_IF.text == string.Empty)
        {
            // If the input field is null, set name as default. 
            ticTacToeManager.SetPlayerName(1, ticTacToeManager.GetPlayer(1).playerName);
        }
        else
        {
            ticTacToeManager.SetPlayerName(1, playerOne_IF.text);
        }

        if (playerOne_IF.text == string.Empty)
        {
            // If the input field is null, set name as default. 
            ticTacToeManager.SetPlayerName(2, ticTacToeManager.GetPlayer(2).playerName);
        }
        else
        {
            ticTacToeManager.SetPlayerName(2, playerTwo_IF.text);
        }
    }

    public void SetPlayerColours()
    {
        ticTacToeManager.SetPlayerColour(1, playerOne.GetComponentInChildren<ColourSwitcher>().GetColour());
        ticTacToeManager.SetPlayerColour(2, playerTwo.GetComponentInChildren<ColourSwitcher>().GetColour());
    }

    #endregion
}
