using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Player", menuName = "Players/Player")]
public class Player : ScriptableObject
{
    [Header("Player Properties")]
    public string playerName;       // The chosen player name.
    public Color32 playerColour;    // The chosen player colour.   
    public Sprite playerIcon;       // The player's icon. Used for Tic-Tac-Toe.

    public bool isCurrentPlayer;    // Whether this player is the current player or not.

    public Color32 defaultColour;   // The default colour of the player.

    public int index;               // The index of the player.
}
