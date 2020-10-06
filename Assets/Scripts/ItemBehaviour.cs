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

        onItemClicked.Invoke(this);
    }

    public IEnumerator Flash(float duration, float frequency, Color color, UnityAction callback)
    {
        alreadyClicked = true;

        ParticleSystemController.instance.PlayParticlesAt(transform);

        float totalDuration = duration;

        while(duration > 0)
        {
            img.color = color;
            yield return new WaitForSeconds(totalDuration / frequency);
            img.color = Color.white;
            yield return new WaitForSeconds(totalDuration / frequency);
            duration -= 2 * totalDuration / frequency;
        }

        callback();
    }

    void Start()
    {
        img = GetComponent<Image>();

        alreadyClicked = false;
    }

    public void ResetProgress()
    {
        alreadyClicked = false;
    }
}

public class OnItemClickedEvent : UnityEvent<ItemBehaviour>
{

}
