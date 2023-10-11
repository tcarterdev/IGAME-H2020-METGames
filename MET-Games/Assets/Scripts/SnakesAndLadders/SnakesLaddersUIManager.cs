using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnakesLaddersUIManager : MonoBehaviour
{
    [Header("Gameplay UI")]
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    [SerializeField] private Button diceButton;
    [SerializeField] private GameObject returnToMenuScreen;

    [Header("Player Names")]
    [SerializeField] private List<TMP_InputField> playerNames_IF;

    [Header("Players")]
    [SerializeField] private List<ColourSwitcher> playerColourSwitchers;

    private SnakesLaddersManager snakesLaddersManager;

    private void Start()
    {
        snakesLaddersManager = FindObjectOfType<SnakesLaddersManager>();
    }

    private void Update()
    {
        if (!snakesLaddersManager.GetHasPlayerWon())
        {
            if (snakesLaddersManager.GetCurrentPlayer() == null) { return; }

            Player currentPlayer = snakesLaddersManager.GetCurrentPlayer();

            currentPlayerText.text = $"{currentPlayer.playerName}'s turn!";
            currentPlayerText.color = currentPlayer.playerColour;

            snakesLaddersManager.GetCurrentPlayerToken().GetComponent<Image>().color = currentPlayer.playerColour;
        }
    }

    /// <summary>
    /// Shows the win message, displaying which player won.
    /// </summary>
    public void ShowWinMessage()
    {
        Player currentPlayer = snakesLaddersManager.GetCurrentPlayer();

        currentPlayerText.text = $"{currentPlayer.playerName} wins!";
        currentPlayerText.color = currentPlayer.playerColour;

        diceButton.gameObject.SetActive(false);
        returnToMenuScreen.SetActive(true);
        //Debug.Log(currentPlayerText.text);
        snakesLaddersManager._MetGamesDataTracking.SnakesAndLadders_PlayerWon(currentPlayer.index);
        snakesLaddersManager._MetGamesDataTracking.GetTimeEnded();
        snakesLaddersManager._MetGamesDataTracking.FinaliseData();
    }

    #region Getters & Setters

    /// <summary>
    /// Sets player names using the input fields.
    /// 
    /// Called by a button. 
    /// </summary>
    public void SetPlayerNames()
    {
        for (int i = 0; i < snakesLaddersManager.GetNumOfPlayers(); i++)
        {
            if (playerNames_IF[i].text == string.Empty)
            {
                snakesLaddersManager.SetPlayerName(i, snakesLaddersManager.GetPlayer(i).playerName);
            }
            else
            {
                snakesLaddersManager.SetPlayerName(i, playerNames_IF[i].text);
            }
        }
    }

    /// <summary>
    /// Sets player colours using the colour switches. 
    /// 
    /// Called by a button. 
    /// </summary>
    public void SetPlayerColours()
    {
        for (int i = 0; i < playerColourSwitchers.Count; i++)
        {
            snakesLaddersManager.SetPlayerColour(i, playerColourSwitchers[i].GetColour());
        }
    }

    /// <summary>
    /// Gets the list of player name input fields, used to update the player names.
    /// </summary>
    /// <returns></returns>
    public List<TMP_InputField> GetPlayerNames_IF() => playerNames_IF;

    #endregion
}
