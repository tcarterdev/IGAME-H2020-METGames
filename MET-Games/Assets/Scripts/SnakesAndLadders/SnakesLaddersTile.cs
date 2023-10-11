using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class SnakesLaddersTile : MonoBehaviour
{
    public int number;
    private TextMeshProUGUI tileText;

    [System.Serializable]
    public enum LinkType { None, Ladder, Snake };

    [SerializeField] private LinkType linkType;
    [SerializeField] private SnakesLaddersTile targetLink;
    [SerializeField] private int targetLinkNumber;
    [SerializeField] private SnakesLaddersTile linkedTo;

    [SerializeField] private Sprite ladderSprite;
    [SerializeField] private Sprite snakeSprite;

    private SnakesLaddersManager snakesLaddersManager;

    // Start is called before the first frame update
    void Start()
    {
        tileText = GetComponentInChildren<TextMeshProUGUI>();
        tileText.text = number.ToString();

        name = $"Tile_{number}";

        snakesLaddersManager = FindObjectOfType<SnakesLaddersManager>();

        snakesLaddersManager.OnTilesSetUp += SnakesLaddersManager_OnTilesSetUp;

        List<SnakesLaddersTile> tiles = snakesLaddersManager.GetTiles();
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].number == targetLinkNumber)
            {
                targetLink = tiles[i];
                targetLink.SetLinkedTo(this);
            }
        }

        if (targetLink != null)
        {
            //Debug.Log($"{this.name}: {this.transform.localPosition}, {targetLink.name}: {targetLink.transform.localPosition}");
            MakeLine(transform.position, targetLink.transform.position);
        }
    }

    /// <summary>
    /// When called, updates the tile's target links and linked to, and creates a line between the two.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SnakesLaddersManager_OnTilesSetUp(object sender, EventArgs e)
    {
        List<SnakesLaddersTile> tiles = snakesLaddersManager.GetTiles();

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].number == targetLinkNumber)    // If the tile number matches the target, set the link properties.
            {
                targetLink = tiles[i];
                targetLink.SetLinkedTo(this);
            }
        }
        
        if (targetLink != null)
        {
            // Create a line between the two tiles to show the link.
            MakeLine(transform.position, targetLink.transform.position);
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
        //if (AudioManager.Instance != null) { AudioManager.Instance.ding.Play(); }

        // Create a new gameObject named "line".
        GameObject newObj = new GameObject();
        
        // Attach an image component.
        Image newImage = newObj.AddComponent<Image>();

        // Update image to be sliced.
        newImage.type = Image.Type.Tiled;

        if (linkType == LinkType.Ladder)
        {
            //newImage.color = Color.magenta;

            // Update the image with the ladder sprite.
            newImage.sprite = ladderSprite;

            newObj.name = $"Ladder_{number}-{targetLinkNumber}";
        }
        else
        {
            newImage.color = Color.green;

            // Update the image with the snake sprite.
            newImage.sprite = snakeSprite;

            newObj.name = $"Snake_{number}-{targetLinkNumber}";
        }

        RectTransform rt = newObj.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;

        // Update point a and b with the passed in start and end positions, exclude the Z.
        Vector3 pointA = new Vector3(_startPos.x, _startPos.y, 0);
        Vector3 pointB = new Vector3(_endPos.x, _endPos.y, 0);

        // Set the local position to the mid-point of pointA and pointB.
        rt.localPosition = (pointA + pointB) / 2;

        // Difference between the two points.
        Vector3 dif = pointA - pointB;
        rt.sizeDelta = new Vector3(dif.magnitude, 15);

        // Rotate the line based on the direction of the win. 
        rt.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));

        if (linkType == LinkType.Snake && rt.rotation.z < 0)
        {
            // Rotate the line based on the direction of the win. 
            rt.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * (Mathf.Atan(dif.y / dif.x) / Mathf.PI) + 180));

        }

        // Set the parent to the canvas transform. 
        rt.SetParent(transform.parent);
        rt.transform.SetAsLastSibling();
    }

    /// <summary>
    /// Sets the number of the tile. 
    /// </summary>
    /// <param name="number">the number of the tile.</param>
    public void SetNumber(int number)
    {
        this.number = number;
        tileText.text = number.ToString();
    }

    /// <summary>
    /// Sets the tile this tile is linked to.
    /// </summary>
    /// <param name="linkedTo">The linked to tile.</param>
    public void SetLinkedTo(SnakesLaddersTile linkedTo) => this.linkedTo = linkedTo;

    /// <summary>
    /// Gets the target tile for the link.
    /// </summary>
    /// <returns></returns>
    public SnakesLaddersTile GetTargetLinkTile() => targetLink;

    /// <summary>
    /// Gets the type of link between tiles.
    /// </summary>
    /// <returns></returns>
    public LinkType GetTileLinkType() => linkType;
}
