using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefab;
    public Transform spawnPoints1;
    public Transform spawnPoints2;
    public Canvas Canvas;
    Draggable draggable;
    public void Start()
    {
        Canvas canvas = (Canvas)GameObject.FindObjectOfType(typeof(Canvas));
        InvokeRepeating("SpawnFood", 4.0f, 10f);
        Draggable draggable = GetComponent<Draggable>();
        
    }

 

    void SpawnFood()
    {
        
        GameObject foodsleft = Instantiate(foodPrefab[Random.Range(0, foodPrefab.Length)], spawnPoints1.transform.position, spawnPoints1.transform.rotation);
        GameObject foodsright = Instantiate(foodPrefab[Random.Range(0, foodPrefab.Length)], spawnPoints2.transform.position, spawnPoints2.transform.rotation);
        foodsleft.transform.parent = Canvas.transform;
        foodsright.transform.parent = Canvas.transform;
        
    }

}
