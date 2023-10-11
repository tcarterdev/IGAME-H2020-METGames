using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinna : MonoBehaviour
{
    public float rotationSpeed;
    
    void Start()
    {
        transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-360f, 360f));
    }

    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, 1) * Time.deltaTime * rotationSpeed;
    }
}
