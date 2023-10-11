using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    // Array of dice side sprites, loaded from Resources folder
    private Sprite[] diceSides;

    // Used to change sprites
    private Image image;

    private int finalSide;

    public event System.EventHandler<int> OnDiceRoll;

    private void Awake()
    {
        // Gets the image component.
        image = GetComponentInChildren<Image>();

        // Loads the sprites for the dice sides.
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
    }

    /// <summary>
    /// Starts the dice roll.
    /// 
    /// Called from a button.
    /// </summary>
    public void StartDiceRoll()
    {
        // Disables the dice button.
        GetComponent<Button>().interactable = false;

        // Starts the dice roll coroutine.
        StartCoroutine(RollDice());
    }

    /// <summary>
    /// Rolls the dice. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator RollDice()
    {
        // Play dice rolling sound.
        AudioManager.Instance.diceRoll.Play();

        // Initialise the random dice roll.
        int randomDiceSide = 0;

        // The final side of the chosen dice roll.
        finalSide = 0;

        // Loop through 20 times, once the max is reached, the result is the last randomDiceSide.
        for (int i = 0; i <= 20; i++)
        {
            // Choose a random side between 1 and 6
            randomDiceSide = Random.Range(0, 5);

            // Updates the dice image with the randomDiceSide index.
            image.sprite = diceSides[randomDiceSide];

            yield return new WaitForSeconds(0.05f);
        }

        // Stop the dice rolling sound.
        AudioManager.Instance.diceRoll.Stop();

        // Play the dice roll finised sound.
        AudioManager.Instance.diceRollFinished.Play();

        // Our final side is the random index + 1 to get the correct result.
        finalSide = randomDiceSide + 1;

        // Invoke the dice roll event.
        OnDiceRoll?.Invoke(this, finalSide);
    }

    /// <summary>
    /// Gets the result of the dice roll.
    /// </summary>
    /// <returns></returns>
    public int GetFinalSide() => finalSide;
}
