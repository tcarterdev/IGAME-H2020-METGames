using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class TicTacToeManager : MonoBehaviour
{
    [SerializeField] private SoundController soundController;

    [Header("Gameplay UI")]
    [SerializeField] private TextMeshProUGUI currentPlayerNameText;
    [SerializeField] private TicTacToeUIManager ticTacToeUIManager;
    [SerializeField] private Sprite lineImage;

    [SerializeField] private Color playerOneColour;
    [SerializeField] private Color playerTwoColour;

    [Header("Player Data")]
    [SerializeField] private Player playerOne;
    [SerializeField] private Player playerTwo;
    private Player currentPlayer;

    [Header("Tiles")]
    [SerializeField] private GameObject[] tiles;

    [Header("Win Conditions")]
    [Header("Rows")]
    [SerializeField] private GameObject[] topRow;
    [SerializeField] private GameObject[] midRow;
    [SerializeField] private GameObject[] botRow;

    [Header("Columns")]
    [SerializeField] private GameObject[] leftCol;
    [SerializeField] private GameObject[] midCol;
    [SerializeField] private GameObject[] rightCol;

    [Header("Diagonals")]
    [SerializeField] private GameObject[] TopLeftToBotRight;
    [SerializeField] private GameObject[] TopRightToBotLeft;

    [Header("Screens")]
    [SerializeField] private GameObject gameBoard;
    [SerializeField] private GameObject returnToMenuScreen;

    [Header("Play Styles")]
    [SerializeField] private GameObject sideBySide;
    [SerializeField] private GameObject across;

    private int turnCounter;
    private bool isPlayerOne;
    private bool isPlayerTwo;
    private bool isAcrossMode;
    private bool hasPlayerWon;
    private bool hasGameStarted;

    public METGamesDataTracking _metGamesDataTracking;

    [DllImport("__Internal")]
    private static extern void ShowMessage(string message);

    private void Start()
    {
        if (soundController != null) { soundController.LoadVolume(); }
    }

    /// <summary>
    /// Starts the game, and sets playerOne to start first.
    /// 
    /// Called by a button.
    /// </summary>
    public void StartGame()
    {
        SetIsPlayerOne();

        ticTacToeUIManager = FindObjectOfType<TicTacToeUIManager>();

        if (isAcrossMode)
        {
            across.SetActive(true);
        }
        else
        {
            sideBySide.SetActive(true);
        }

        gameBoard.SetActive(true);

        hasGameStarted = true;
    }

    /// <summary>
    /// Checks each of the win conditions. 
    /// </summary>
    public void CheckGameState()
    {
        // Rows
        CheckRows();

        // Cols
        CheckColumns();

        // Diags
        CheckDiagonals();

        turnCounter++;

        if (turnCounter == 9 && !hasPlayerWon)
        {
        #if !UNITY_EDITOR
            // Send to JavaScript.
            ShowMessage("Game Lost");
        #endif

            // It's a tie.
            ShowBackToMenu();

            _metGamesDataTracking.TicTacToeComplete(2, true);
            ticTacToeUIManager.ShowTieMessage();
        }
    }

    private void CheckRows()
    {
        // Top Row
        int playerOneCount = 0;
        int playerTwoCount = 0;
        for (int i = 0; i < topRow.Length; i++)
        {
            if (topRow[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerOne) { playerOneCount++; }
            if (topRow[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerTwo) { playerTwoCount++; }
        }

        // Check if a player has won.
        if (CheckIfPlayerWon(playerOneCount, playerTwoCount))
        {
            // If they have, create a line to show the win.
            MakeLine(topRow[0].transform.position, topRow[2].transform.position);
        }

        // Middle Row
        playerOneCount = 0;
        playerTwoCount = 0;
        for (int i = 0; i < midRow.Length; i++)
        {
            if (midRow[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerOne) { playerOneCount++; }
            if (midRow[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerTwo) { playerTwoCount++; }
        }

        // Check if a player has won.
        if (CheckIfPlayerWon(playerOneCount, playerTwoCount))
        {
            // If they have, create a line to show the win.
            MakeLine(midRow[0].transform.position, midRow[2].transform.position);
        }

        // Bottom Row
        playerOneCount = 0;
        playerTwoCount = 0;
        for (int i = 0; i < botRow.Length; i++)
        {
            if (botRow[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerOne) { playerOneCount++; }
            if (botRow[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerTwo) { playerTwoCount++; }
        }

        // Check if a player has won.
        if (CheckIfPlayerWon(playerOneCount, playerTwoCount))
        {
            // If they have, create a line to show the win.
            MakeLine(botRow[0].transform.position, botRow[2].transform.position);
        }
    }

    /// <summary>
    /// Checks for a win in the columns.
    /// </summary>
    private void CheckColumns()
    {
        // Left Column
        int playerOneCount = 0;
        int playerTwoCount = 0;
        for (int i = 0; i < leftCol.Length; i++)
        {
            if (leftCol[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerOne) { playerOneCount++; }
            if (leftCol[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerTwo) { playerTwoCount++; }
        }

        // Check if a player has won.
        if (CheckIfPlayerWon(playerOneCount, playerTwoCount))
        {
            // If they have, create a line to show the win.
            MakeLine(leftCol[0].transform.position, leftCol[2].transform.position);
        }

        // Middle Column
        playerOneCount = 0;
        playerTwoCount = 0;
        for (int i = 0; i < midCol.Length; i++)
        {
            if (midCol[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerOne) { playerOneCount++; }
            if (midCol[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerTwo) { playerTwoCount++; }
        }

        // Check if a player has won.
        if (CheckIfPlayerWon(playerOneCount, playerTwoCount))
        {
            // If they have, create a line to show the win.
            MakeLine(midCol[0].transform.position, midCol[2].transform.position);
        }

        // Bottom Column
        playerOneCount = 0;
        playerTwoCount = 0;
        for (int i = 0; i < rightCol.Length; i++)
        {
            if (rightCol[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerOne) { playerOneCount++; }
            if (rightCol[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerTwo) { playerTwoCount++; }
        }

        // Check if a player has won.
        if (CheckIfPlayerWon(playerOneCount, playerTwoCount))
        {
            // If they have, create a line to show the win.
            MakeLine(rightCol[0].transform.position, rightCol[2].transform.position);
        }
    }

    /// <summary>
    /// Checks for a win in the diagonals. 
    /// </summary>
    private void CheckDiagonals()
    {
        // Top Left to Bottom Right
        int playerOneCount = 0;
        int playerTwoCount = 0;
        for (int i = 0; i < TopLeftToBotRight.Length; i++)
        {
            if (TopLeftToBotRight[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerOne) { playerOneCount++; }
            if (TopLeftToBotRight[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerTwo) { playerTwoCount++; }
        }

        // Check if a player has won.
        if (CheckIfPlayerWon(playerOneCount, playerTwoCount))
        {
            // If they have, create a line to show the win.
            MakeLine(TopLeftToBotRight[0].transform.position, TopLeftToBotRight[2].transform.position);
        }

        // Top Right to Bottom Left
        playerOneCount = 0;
        playerTwoCount = 0;
        for (int i = 0; i < TopRightToBotLeft.Length; i++)
        {
            if (TopRightToBotLeft[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerOne) { playerOneCount++; }
            if (TopRightToBotLeft[i].GetComponent<TicTacToeTile>().GetWhoClaimed() == playerTwo) { playerTwoCount++; }
        }

        // Check if a player has won.
        if (CheckIfPlayerWon(playerOneCount, playerTwoCount))
        {
            // If they have, create a line to show the win.
            MakeLine(TopRightToBotLeft[0].transform.position, TopRightToBotLeft[2].transform.position);
        }
    }

    /// <summary>
    /// Creates a line between the three tiles to show how the game was won.
    /// </summary>
    /// <param name="_startPos">the start position tile.</param>
    /// <param name="_endPos">the end position tile.</param>
    private void MakeLine(Vector3 _startPos, Vector3 _endPos)
    {
        // Play sound on card match.
        if (AudioManager.Instance != null) { AudioManager.Instance.ding.Play(); }

        // Create a new gameObject named "line".
        GameObject newObj = new GameObject();
        newObj.name = "line";

        // Attach an image component.
        Image newImage = newObj.AddComponent<Image>();

        // Update the image with our line sprite, and update the colour to the winning player's colour. 
        newImage.sprite = lineImage;
        newImage.color = currentPlayer.playerColour;
        RectTransform rect = newObj.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;

        // Update point a and b with the passed in start and end positions, exclude the Z.
        Vector3 pointA = new Vector3(_startPos.x, _startPos.y, 0);
        Vector3 pointB = new Vector3(_endPos.x, _endPos.y, 0);

        // Set the local position to the mid-point of pointA and pointB.
        rect.localPosition = (pointA + pointB) / 2;

        // Difference between the two points.
        Vector3 dif = pointA - pointB;
        rect.sizeDelta = new Vector3(dif.magnitude, 3);

        // Rotate the line based on the direction of the win. 
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));

        // Set the parent to the canvas transform. 
        rect.SetParent(gameBoard.transform);
    }

    /// <summary>
    /// Checks if a player has won or not. 
    /// 
    /// If the count for either player is 3, game has been won.
    /// </summary>
    /// <param name="playerOneCount">the number of tiles matching for playerOne.</param>
    /// <param name="playerTwoCount">the number of tiles matching for playerTwo.</param>
    /// <returns></returns>
    private bool CheckIfPlayerWon(int playerOneCount, int playerTwoCount) 
    {
        if (playerOneCount == 3)
        {
            // playerOne has won.
            hasPlayerWon = true;

            // Show the win message for player one.
            ticTacToeUIManager.ShowWinMessage(1);
            _metGamesDataTracking.TicTacToeComplete(0, false);

        #if !UNITY_EDITOR
            // Send to JavaScript.
            ShowMessage("Game Won");
        #endif

            DisableTiles();

            ShowBackToMenu();

            return true;
        }
        else if (playerTwoCount == 3)
        {
            // playerTwo has won.
            hasPlayerWon = true;

            // Show the win message for player two.
            ticTacToeUIManager.ShowWinMessage(2);
            _metGamesDataTracking.TicTacToeComplete(1, false);
        #if !UNITY_EDITOR
            // Send to JavaScript.
            ShowMessage("Game Lost");
        #endif

            DisableTiles();

            ShowBackToMenu();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Disables the tiles, and hides the ones that have no image. 
    /// </summary>
    private void DisableTiles()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            // Make the tile non-interactable.
            tiles[i].GetComponent<Button>().interactable = false;

            // Hide the valid tile gameObject.
            tiles[i].transform.Find("Valid").gameObject.SetActive(false);

            // If the sprite is null, hide the gameObject.
            if (tiles[i].transform.Find("Image").GetComponent<Image>().sprite == null) { tiles[i].transform.Find("Image").gameObject.SetActive(false); }
        }
    }

    /// <summary>
    /// Shows the back to menu gameObject. 
    /// </summary>
    private void ShowBackToMenu()
    {
        returnToMenuScreen.SetActive(true);
    }

    #region Getters & Setters

    /// <summary>
    /// Sets Player One's name.
    /// </summary>
    /// <param name="name">The name for player one.</param>
    public void SetPlayerOneName(string name) => playerOne.playerName = name;

    /// <summary>
    /// Sets Player Two's name.
    /// </summary>
    /// <param name="name">The name for player two.</param>
    public void SetPlayerTwoName(string name) => playerTwo.playerName = name;

    public void SetPlayerName(int playerIndex, string playerName)
    {
        if (playerIndex == 1)
        {
            playerOne.playerName = playerName;
        }
        else
        {
            playerTwo.playerName = playerName;
        }
    }
    
    /// <summary>
    /// Set playerOne as active. 
    /// </summary>
    public void SetIsPlayerOne()
    {
        isPlayerOne = true;
        isPlayerTwo = false;

        currentPlayer = playerOne;
    }

    /// <summary>
    /// Set playerTwo as active. 
    /// </summary>
    public void SetIsPlayerTwo()
    {
        isPlayerTwo = true;
        isPlayerOne = false;

        currentPlayer = playerTwo;
    }

    public void SetPlayerColour(int playerIndex, Color colour)
    {
        if (playerIndex == 1)
        {
            playerOne.playerColour = colour;
        }
        else
        {
            playerTwo.playerColour = colour;
        }
    }

    /// <summary>
    /// Gets if current player is playerOne.
    /// </summary>
    /// <returns></returns>
    public bool GetIsPlayerOne() => isPlayerOne;

    /// <summary>
    /// Gets the current player. 
    /// </summary>
    /// <returns></returns>
    public Player GetCurrentPlayer() => currentPlayer;

    public Player GetPlayerOne() => playerOne;
    public Player GetPlayerTwo() => playerTwo;

    public Player GetPlayer(int index)
    {
        switch (index)
        {
            case 1: return playerOne;

            case 2: return playerTwo;
        }

        return null;
    }

    /// <summary>
    /// Gets if a player has won or not.
    /// </summary>
    /// <returns></returns>
    public bool GetHasPlayerWon() => hasPlayerWon;

    /// <summary>
    /// Sets if the play style is Across or not.
    /// </summary>
    /// <param name="state"></param>
    public void SetIsAcrossMode(bool state) => isAcrossMode = state;

    public bool GetIsAcrossMode() => isAcrossMode;

    public int GetTurnCounter() => turnCounter;

    public bool GetHasGameStarted() => hasGameStarted;

    #endregion

    private void OnDisable()
    {
        // Reset the player names back to Player #1 and Player #2.
        playerOne.playerName = "Player #1";
        playerTwo.playerName = "Player #2";

        // Reset the player colours back to default (red and blue)
        playerOne.playerColour = playerOne.defaultColour;
        playerTwo.playerColour = playerTwo.defaultColour;
    }


    public void LeftEarly() 
    {
        _metGamesDataTracking.GetTimeEnded();
        _metGamesDataTracking.DS_Timings.GameStatus = METGamesDataTracking.GameStatus.EXITED;
        _metGamesDataTracking.DS_TicTacToe.PlayerOneGameStatus = METGamesDataTracking.GameStatus.EXITED;
        _metGamesDataTracking.DS_TicTacToe.PlayerTwoGameStatus = METGamesDataTracking.GameStatus.EXITED;
        _metGamesDataTracking.FinaliseData();
    }
}
