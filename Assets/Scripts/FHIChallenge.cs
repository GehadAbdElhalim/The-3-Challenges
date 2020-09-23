using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RTLTMPro;

public class FHIChallenge : Challenge
{
    AudioSource audio;

    [Header("Specific parameters for this challenge")]
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

    [Header("SFX")]
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
        ChooseRandomObject();

        Invoke("StartTimer", Time.deltaTime);
    }

    void StartTimer()
    {
        if (!ChallengeManager.Instance.tutorial.activeSelf)
        {
            TimerController.Instance.StartTimer(seconds, minutes, null, null);
        }
    }

    public override void ResetProgress()
    {
        DisableAllTexts();
        ChooseRandomObject();
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
