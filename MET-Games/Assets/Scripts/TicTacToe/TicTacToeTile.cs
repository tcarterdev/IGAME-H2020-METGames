using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeTile : MonoBehaviour
{
    [SerializeField] private Image tileImage;
    [SerializeField] private Sprite noughtSprite;
    [SerializeField] private Sprite crossSprite;

    [SerializeField] private GameObject validSpace;     // Green square will show

    [SerializeField] private Button tileButton;

    private TicTacToeManager ticTacToeManager;

    private Player claimedBy;
    private Animator animator;

    private void Start()
    {
        ticTacToeManager = FindObjectOfType<TicTacToeManager>();
        animator = GetComponent<Animator>();

        // Ensure Tiles are in a default state. 
        validSpace.SetActive(true);
        tileButton.interactable = true;
    }

    /// <summary>
    /// Claims a tile. 
    /// 
    /// Called by a button.
    /// </summary>
    public void ClaimTile()
    {
        // Stop tile animation.
        animator.enabled = false;

        // Increase alpha of the image (it is low by default in inspector).
        var tempColor = tileImage.color;
        tempColor.a = 1f;
        tileImage.color = tempColor;

        // Reset scale back to default.
        transform.localScale = new Vector3(1, 1, 1);

        // Update the sprite and colour to the current player. 
        tileImage.sprite = ticTacToeManager.GetCurrentPlayer().playerIcon;
        tileImage.color = ticTacToeManager.GetCurrentPlayer().playerColour;

        // Deactivate valid space. 
        validSpace.SetActive(false);

        // Disable tile. 
        tileButton.interactable = false;

        // Set the claimedBy to the current player. 
        claimedBy = ticTacToeManager.GetCurrentPlayer();

        // Check if the game has been won.
        ticTacToeManager.CheckGameState();

        // Change the current player. 
        if (ticTacToeManager.GetIsPlayerOne()) 
        { 
            ticTacToeManager.SetIsPlayerTwo();
        }
        else 
        {
            ticTacToeManager.SetIsPlayerOne();
        }
    }

    /// <summary>
    /// Gets who the tile was claimed by. 
    /// </summary>
    /// <returns></returns>
    public Player GetWhoClaimed() => claimedBy;
}
