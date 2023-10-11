using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGrid : MonoBehaviour
{
    public static MatchGrid Instance { get; private set; }
    public event EventHandler OnGridCreated;

    [Min(2)]
    [SerializeField] private int gridX;

    [Min(2)]
    [SerializeField] private int gridY;

    [SerializeField] private float gridOffset; 

    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private bool isGridSquare;

    private GameObject gridSizeScreen;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one MatchGrid! {transform} - {Instance}");
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        gridSizeScreen = GameObject.Find("MatchCanvas").transform.Find("GridSizeScreen").gameObject;
    }

    /// <summary>
    /// Adjust the grid size if an odd number was provided within the inspector. 
    /// </summary>
    private void AdjustGridSize()
    {
        // Adjust grid value if it is an odd number
        if (gridX % 2 != 0)
        {
            gridX++;
            gridY++;
        }
    }

    /// <summary>
    /// Adjusts the camera to fit the grid on the screen, 
    /// by adjusting its orthographic size and position. 
    /// </summary>
    private void AdjustCamera()
    {
        // Adjust camera position 
        if (isGridSquare)
        {
            if (gridX == 2)
            {
                Camera.main.orthographicSize = 3; 
                Camera.main.transform.position = new Vector3(-1.15f, -1.25f, -10f);
            }
            else if (gridX == 4)
            {
                Camera.main.orthographicSize = 5;
                Camera.main.transform.position = new Vector3(0f, 0f, -10f);
            }
            else if (gridX == 6)
            {
                Camera.main.orthographicSize = 7.5f;
                Camera.main.transform.position = new Vector3(1.15f, 0f, -10f);
            }
        }
        else
        {
            if (gridX == 4 && gridY == 6)
            {
                Camera.main.orthographicSize = 5;
                Camera.main.transform.position = new Vector3(0f, 1.25f, -10f);
            }
        }
    }

    /// <summary>
    /// Creates a grid based on the sizes provided, 
    /// and instatiates the card objects. 
    /// </summary>
    private void CreateGrid()
    {
        // Reset grid position. Prevents cards being offset too far, when restarting the game. 
        transform.position = new Vector2(0f, 0f);

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                // Create card at position
                GameObject go = Instantiate(cardPrefab, transform);

                // Update position, along with offset 
                go.transform.position = new Vector2(x * gridOffset, y * gridOffset);
            }
        }

        // Adjust position of grid
        transform.position = new Vector2(-1.725f, -1.725f);

        // Invoke the grid created event now that is has been created.
        OnGridCreated?.Invoke(this, EventArgs.Empty);
    }

    public void SetUpGrid()
    {
        if (gridSizeScreen.activeInHierarchy)
        {
            // Hide the grid size selection screen. 
            gridSizeScreen.SetActive(false);
        }

        // If the grid selected is square, update gridY with gridX. 
        if (isGridSquare)
        {
            gridY = gridX;

            AdjustGridSize();
        }

        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<TimeToBeat>() != null)
        {
            // Update the times to beat text. 
            MatchGameManager.Instance.GetComponent<TimedUI>()
                .SetTimeText(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<TimeToBeat>().GetTimes());
        }

        AdjustCamera();

        CreateGrid();
    }

    public void ResetGrid()
    {
        // Reset cards list. 
        MatchGameManager.Instance.ResetCardsList();

        // Reset cards flipped counter. 
        MatchGameManager.Instance.ResetCardFlipCounter();

        // Temp parent to attach the cards to delete to. 
        GameObject newParent = new GameObject("Temp Parent");
        
        // Unparent the cards. 
        for (int i = GetGridSize() - 1; i <= transform.childCount; i--)
        {
            if (i < 0) { break; }

            transform.GetChild(i).parent = newParent.transform;
        }

        // Destroy card objects.
        for (int i = 0; i < newParent.transform.childCount; i++)
        {
            Destroy(newParent.transform.GetChild(i).gameObject);
        }

        // Destroy temp parent gameobject.
        Destroy(newParent);

        // Reset card icon counters.
        MatchGameManager.Instance.ResetCardIconCounters();

        MatchGameManager.Instance.ResetScore();

        SetUpGrid();
    }


    #region Getters & Setters

    /// <summary>
    /// Calculates and returns the grid size based on the multiplication of X and Y. 
    /// </summary>
    /// <returns>the grid size. </returns>
    public int GetGridSize() => gridX * gridY;

    public int GetGridX() => gridX;
    public int GetGridY() => gridY;

    public bool GetIsGridSquare() => isGridSquare;

    /// <summary>
    /// Called from the buttons within the grid size selection screen,
    /// and sets the size of the grid to be created for that game. 
    /// </summary>
    /// <param name="size">The size of the selected grid.</param>
    public void SetGridSize(int size)
    {
        // Update gridX with the new size. 
        gridX = size;
    }

    /// <summary>
    /// Called from the buttons within the grid size selection screen, 
    /// and sets whether the grid is meant to be a square or not. 
    /// </summary>
    /// <param name="state"></param>
    public void SetIsGridSquare(bool state) => isGridSquare = state;

    public void SetGridX(int gridX) => this.gridX = gridX;

    public void SetGridY(int gridY) => this.gridY = gridY;

    #endregion
}
