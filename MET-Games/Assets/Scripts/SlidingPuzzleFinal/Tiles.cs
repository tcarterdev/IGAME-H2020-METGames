using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public Vector3 targetPosition;
    private Vector3 correctPosition;
    private SpriteRenderer tileSprite;
    public int number;

    public bool inRightPlace;


    void Awake()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
        tileSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.10f);
        if (targetPosition == correctPosition)
        {
            tileSprite.color = Color.green;
            inRightPlace = true;
        }
        else
        {
            tileSprite.color = Color.white;
            inRightPlace = false;
        }
    }

    public Vector3 GetCorrectPos() => correctPosition;
    
}

