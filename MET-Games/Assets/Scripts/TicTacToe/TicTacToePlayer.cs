using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_TicTacToe_Player", menuName = "TicTacToe/TicTacToe_Player")]
public class TicTacToePlayer : ScriptableObject
{
    [Header("Player Properties")]
    public string playerName;
    public Color32 playerColour;
    public Sprite playerIcon;

    public bool isCurrentPlayer;

    public Color32 defaultColour;
}
