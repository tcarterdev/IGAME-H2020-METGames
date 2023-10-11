using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ColourSwitcher : MonoBehaviour
{
    private PlayerColour playerColour;

    [SerializeField] private int startColourIndex;

    [Header("Player Colours")]
    [SerializeField] private Image colourImage;
    private List<Color32> colours;
    private int currentColourIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerColour = FindObjectOfType<PlayerColour>();

        colours = playerColour.GetColours();

        colourImage.color = colours[startColourIndex];
        currentColourIndex = startColourIndex;
    }

    public void CycleColour(int direction)
    {
        if (currentColourIndex + direction > colours.Count - 1)
        {
            currentColourIndex = 0;
            colourImage.color = colours[currentColourIndex];
            return;
        }
        else if (currentColourIndex + direction < 0)
        {
            currentColourIndex = colours.Count - 1;
            colourImage.color = colours[currentColourIndex];
            return;
        }

        colourImage.color = colours[currentColourIndex + direction];
        currentColourIndex = currentColourIndex + direction;
    }

    public Color32 GetColour() => colourImage.color;
}
