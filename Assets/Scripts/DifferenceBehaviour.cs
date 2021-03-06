﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifferenceBehaviour : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int index;

    public float fillSpeed;

    Image image;

    bool isActivated;

    public DifferenceCheckedEvent OnChecked = new DifferenceCheckedEvent();
    public UnityEvent OnCheckedFinished = new UnityEvent();

    public Color hintColor;

    private Color originalColor;

    private void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0;
        originalColor = image.color;

        isActivated = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isActivated)
        {
            return;
        }

        isActivated = true;

        OnChecked.Invoke(index);

        StartCoroutine(FillImage(true));
    }

    IEnumerator FillImage(bool invokeOnCheckedEvent)
    {
        RemoveHint();

        while(image.fillAmount < 1)
        {
            image.fillAmount += Time.deltaTime * fillSpeed;
            image.fillAmount = Mathf.Clamp01(image.fillAmount);
            yield return new WaitForEndOfFrame();
        }

        if (invokeOnCheckedEvent)
        {
            OnCheckedFinished.Invoke();
        }
    }

    public void Activate()
    {
        if (isActivated)
        {
            return;
        }

        isActivated = true;

        StartCoroutine(FillImage(false));
    }

    public void DeActivate()
    {
        isActivated = false;
        image.fillAmount = 0;
    }

    public bool IsAlreadyActive()
    {
        return isActivated;
    }

    public void ShowHint()
    {
        image.color = hintColor;
        image.fillAmount = 1;
    }

    void RemoveHint()
    {
        image.fillAmount = 0;
        image.color = originalColor;
    }
}

public class DifferenceCheckedEvent : UnityEvent<int>
{

}
