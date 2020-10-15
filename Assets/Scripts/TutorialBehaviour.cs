using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBehaviour : MonoBehaviour
{
    private static TutorialBehaviour _instance;

    public static TutorialBehaviour Instance
    {
        set
        {
            _instance = FindObjectOfType<TutorialBehaviour>();
        }

        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<TutorialBehaviour>();
            }

            return _instance;
        }
    }

    public GameObject[] Slides;

    private int index = 0;

    private void Awake()
    {
        _instance = this;
    }

    private void OnEnable()
    {
        NextSlide();
        TimerController.Instance?.PauseTimer();
    }

    private void OnDisable()
    {
        DisableAllSlides();
        index = 0;
        TimerController.Instance.UnpauseTimer();
    }

    public void NextSlide()
    {
        if(index >= Slides.Length)
        {
            gameObject.SetActive(false);
            return;
        }

        DisableAllSlides();
        Slides[index++].SetActive(true);
    }

    void DisableAllSlides()
    {
        foreach(var slide in Slides)
        {
            slide.SetActive(false);
        }
    }
}
