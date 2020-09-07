﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FHIChallenge : Challenge
{
    AudioSource audio;

    public OnItemChosenEvent OnItemChosen = new OnItemChosenEvent();

    public List<ItemBehaviour> items;
    public List<string> texts;

    [Header("Duration of the challenge")]
    public int minutes;
    public int seconds;

    [Header("Flashing settings")]
    public float duration;
    public float frequency;
    public Color flashColor;

    ItemBehaviour currentItem;
    string currentText;

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
        OnItemChosen.Invoke(currentText);
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
        TimerController.Instance.StartTimer(seconds, minutes, null, null);
    }

    public override void ResetProgress()
    {
        TimerController.Instance.StartTimer(seconds, minutes, null, null);
    }
}

[System.Serializable]
public class OnItemChosenEvent : UnityEvent<string>
{

}