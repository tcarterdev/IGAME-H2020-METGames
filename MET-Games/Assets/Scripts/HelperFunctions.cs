using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
    public static HelperFunctions Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"There's more than one HelperFunctions! {transform} - {Instance}");
            Destroy(gameObject);
        }

        Instance = this;
    }

    public int BoolToInt(bool state)
    {
        if (state)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public bool IntToBool(int value)
    {
        if (value != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
