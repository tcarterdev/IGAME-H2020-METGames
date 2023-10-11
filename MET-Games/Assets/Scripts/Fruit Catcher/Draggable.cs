using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startingPosition;
    [Space]
    public bool xLock;
    public bool yLock;
    public bool findSingleton = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (findSingleton == true)
        {
            canvas = CanvasSingleton.Instance.GetComponent<Canvas>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startingPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (yLock)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startingPosition.y);
        }
        
        if (xLock)
        {
            rectTransform.anchoredPosition = new Vector2(startingPosition.x, rectTransform.anchoredPosition.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = startingPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}