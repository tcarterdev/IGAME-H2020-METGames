using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColour : MonoBehaviour
{
    [SerializeField] private Color32 iGameColour;

    [Header("Player Colours")]
    [SerializeField] private List<Color32> colours;

    public List<Color32> GetColours() => colours;

    public Color32 GetIGameColour() => iGameColour;
}
