using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FruitEnum : MonoBehaviour, IBeginDragHandler
{
    public string type;
    Trolley trolley;
    public void Awake()
    {
      trolley = GameObject.FindGameObjectWithTag("Trolley").GetComponent<Trolley>();

        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        trolley.fenum = this;
    }
}
