using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    public OnItemClickedEvent onItemClicked = new OnItemClickedEvent();

    bool alreadyClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (alreadyClicked)
        {
            return;
        }

        alreadyClicked = true;

        onItemClicked.Invoke(this);
    }

    void Start()
    {
        alreadyClicked = false;
    }
}

public class OnItemClickedEvent : UnityEvent<ItemBehaviour>
{

}
