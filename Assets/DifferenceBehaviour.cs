using System.Collections;
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

    private void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0;

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

        StartCoroutine(FillImage());
    }

    IEnumerator FillImage()
    {
        while(image.fillAmount < 1)
        {
            image.fillAmount += Time.deltaTime * fillSpeed;
            image.fillAmount = Mathf.Clamp01(image.fillAmount);
            yield return new WaitForEndOfFrame();
        }
    }

    public void Activate()
    {
        if (isActivated)
        {
            return;
        }

        isActivated = true;

        StartCoroutine(FillImage());
    }
}

public class DifferenceCheckedEvent : UnityEvent<int>
{

}
