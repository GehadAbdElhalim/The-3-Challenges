using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    public OnItemClickedEvent onItemClicked = new OnItemClickedEvent();

    bool alreadyClicked;

    Image img;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (alreadyClicked)
        {
            return;
        }

        alreadyClicked = true;

        StartCoroutine(Flash(2, 5, Color.green));
    }

    IEnumerator Flash(float duration, float frequency, Color color)
    {
        float totalDuration = duration;

        while(duration > 0)
        {
            img.color = color;
            yield return new WaitForSeconds(totalDuration / frequency);
            img.color = Color.white;
            yield return new WaitForSeconds(totalDuration / frequency);
            duration -= 2 * totalDuration / frequency;
        }

        onItemClicked.Invoke(this);
    }

    void Start()
    {
        img = GetComponent<Image>();

        alreadyClicked = false;
    }
}

public class OnItemClickedEvent : UnityEvent<ItemBehaviour>
{

}
