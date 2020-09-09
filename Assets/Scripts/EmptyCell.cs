using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EmptyCell : MonoBehaviour, IDropHandler
{
    public OnPiecePut OnPiecePut = new OnPiecePut();

    public int index;
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            PuzzleItem pi = eventData.pointerDrag.GetComponent<PuzzleItem>();

            if(pi != null)
            {
                OnPiecePut.Invoke(index, pi.Index);
            }
        }
    }
}

public class OnPiecePut : UnityEvent<int , int>
{

}