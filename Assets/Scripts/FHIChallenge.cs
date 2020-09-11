using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RTLTMPro;

public class FHIChallenge : Challenge
{
    AudioSource audio;

    public List<ItemBehaviour> items;
    public List<GameObject> texts;

    [Header("Duration of the challenge")]
    public int minutes;
    public int seconds;

    [Header("Flashing settings")]
    public float duration;
    public float frequency;
    public Color flashColor;

    ItemBehaviour currentItem;
    GameObject currentText;

    public AudioClip correct_sfx;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        foreach(var x in items)
        {
            x.onItemClicked.AddListener(CheckForCorrectItem);
        }

        ChooseRandomObject();
    }

    void ChooseRandomObject()
    {
        int index = Random.Range(0, items.Count);
        currentItem = items[index];
        currentText = texts[index];
        ActivateCurrentText();
    }

    void CheckForCorrectItem(ItemBehaviour item)
    {
        if(item.GetInstanceID() == currentItem.GetInstanceID())
        {
            audio.PlayOneShot(correct_sfx);
            StartCoroutine(item.Flash(duration, frequency, flashColor, () => OnChallengeFinished.Invoke()));
        }
    }

    private void OnEnable()
    {
        if (!ChallengeManager.Instance.tutorial.activeSelf)
        {
            TimerController.Instance.StartTimer(seconds, minutes, null, null);
        }
        ChooseRandomObject();
    }

    public override void ResetProgress()
    {
        DisableAllTexts();
        TimerController.Instance.StartTimer(seconds, minutes, null, null);
    }

    void DisableAllTexts()
    {
        foreach(var text in texts)
        {
            text.SetActive(false);
        }
    }

    void ActivateCurrentText()
    {
        DisableAllTexts();
        currentText.SetActive(true);
    }
}
