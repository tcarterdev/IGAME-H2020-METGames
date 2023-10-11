using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private FruitLauncher fruitLauncher;
    [SerializeField] private AudioSource audioSource;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Fruit") { return; }

        Destroy(other.gameObject);
        fruitLauncher.fruitCaught += 1;
        audioSource.Play();
    }
}
