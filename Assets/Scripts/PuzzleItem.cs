using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PuzzleItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public UnityEvent OnPiecePickedUp = new UnityEvent();
    [HideInInspector] public OnPieceThrown OnPieceThrown = new OnPieceThrown();

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 originalPos;

    public int Index;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetOriginalPosition()
    {
        originalPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnPiecePickedUp.Invoke();
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / CanvasSingleton.instance.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.GetComponent<EmptyCell>() == null)
        {
            OnPieceThrown.Invoke(Index);
            transform.position = originalPos;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}

public class OnPieceThrown : UnityEvent<int>
{

}
