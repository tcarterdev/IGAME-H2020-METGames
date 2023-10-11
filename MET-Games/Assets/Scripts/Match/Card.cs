using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private float flipSpeed;

    private Canvas cardCanvas;
    private Animator cardAnim;

    private GameObject cardFront;
    private GameObject cardIcon;
    private GameObject cardBg;
    private GameObject cardBack;

    private bool cardFlipped;

    private void Awake()
    {
        cardFront = transform.Find("Front").gameObject;
        cardIcon = cardFront.transform.Find("Icon").gameObject;
        cardBg = cardFront.transform.Find("BG").gameObject;

        cardBack = transform.Find("Back").gameObject;

        cardAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        // Hides the card background and canvas
        cardBg.SetActive(false);
        cardFront.SetActive(false);

        // Hides the card icon by setting the image's alpha to zero. 
        Color cardIconColour = cardIcon.GetComponent<SpriteRenderer>().color;
        cardIconColour.a = 0f;
        cardIcon.GetComponent<SpriteRenderer>().color = cardIconColour;
    }


    private void OnMouseDown()
    {
        // If the card has already been flipped, or the maxmimum number of cards flipped has been reached, return early. 
        if (MatchGameManager.Instance.GetIsUiOpen() || cardFlipped || MatchGameManager.Instance.GetCardFlipCounter() == MatchGameManager.Instance.GetNumberToMatch()) { return; }

        // Set card flipped to true, so it can be flipped again, and increase the card flip counter. 
        cardFlipped = true;
        MatchGameManager.Instance.IncreaseCardFlipCounter();

        // Start the card flipping animation. 
        cardAnim.SetBool("isCardClicked", true);
    }

    /// <summary>
    /// Called from the flip animation, as an animation event. 
    /// 
    /// Hides the card back, and reveals the front of the card, 
    /// then adds it to the list to compare against. 
    /// </summary>
    private void Flip()
    {
        // Hide card back
        cardBack.SetActive(false);

        // Show card background and canvas (for the icon)
        cardBg.SetActive(true);
        cardFront.SetActive(true);

        // Reveal the card icon 
        Color cardIconColour = cardIcon.GetComponent<SpriteRenderer>().color;
        cardIconColour.a = 255f;
        cardIcon.GetComponent<SpriteRenderer>().color = cardIconColour;

        // Add to the card list
        MatchGameManager.Instance.AddCardToList(gameObject);
    }

    /// <summary>
    /// Called from the reverse flip animation, as an animation event. 
    /// 
    /// Hides the card front, and reveals the back of the card, 
    /// then resets card state. 
    /// </summary>
    private void HideFront()
    {
        cardBack.SetActive(true);

        cardBg.SetActive(false);
        cardFront.SetActive(false);

        Color cardIconColour = cardIcon.GetComponent<SpriteRenderer>().color;
        cardIconColour.a = 0f;
        cardIcon.GetComponent<SpriteRenderer>().color = cardIconColour;

        MatchGameManager.Instance.ResetCardFlipCounter();
        cardFlipped = false;
    }

    #region Getters & Setters

    /// <summary>
    /// Gets the icon of the card. 
    /// </summary>
    /// <returns>A sprite of the card's icon.</returns>
    public Sprite GetCardIcon() => cardIcon.GetComponent<SpriteRenderer>().sprite;

    /// <summary>
    /// Sets the icon of the card.
    /// </summary>
    /// <param name="icon">The sprite to update the card's icon with.</param>
    public void SetCardIcon(Sprite icon) => cardIcon.GetComponent<SpriteRenderer>().sprite = icon;

    #endregion
}
