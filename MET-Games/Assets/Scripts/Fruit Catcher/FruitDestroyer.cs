using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDestroyer : MonoBehaviour
{
    [SerializeField] private FruitLauncher fruitLauncher;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Fruit") { return; }

        Destroy(other.gameObject);
        fruitLauncher.fruitMissed += 1;
    }
}
