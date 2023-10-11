using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListGeneration : MonoBehaviour
{
    

    public List<string> foodItems = new List<string>();

    public List<string> RandomFoodList = new List<string>();

    public TMP_Text[] itemtext; 
    

    public void Start()
    {
        foreach (TMP_Text text in itemtext)
        {
            int randnum = Random.Range(0, foodItems.Count);
            //Generates Item 
            text.SetText(foodItems[randnum]);
            RandomFoodList.Add(foodItems[randnum]);
            //Removes Duplicates 
            foodItems.RemoveAt(randnum);
        }

    }

    
    void Update()
    {
        

        
    }
}
