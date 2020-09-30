using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using RTLTMPro;

public class FHIChallenge : Challenge
{
    AudioSource audio;

    [Header("Specific parameters for this challenge")]
    public List<ItemBehaviour> items;
    public List<GameObject> texts;
    public int numberOfHiddenItems;

    [Header("Duration of the challenge")]
    public int minutes;
    public int seconds;

    [Header("Flashing settings")]
    public float duration;
    public float frequency;
    public Color flashColor;

    int[] indicesOfChosenItems;
    int indexOfIndicesOfChosenItems;
    int currentItemIndex;
    bool isFirstTimeDone = false;

    [Header("SFX")]
    public AudioClip correct_sfx;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        foreach(var x in items)
        {
            x.onItemClicked.AddListener(CheckForCorrectItem);
        }
    }

    void ChooseRandomObject()
    {
        if (!isFirstTimeDone)
        {
            int[] arrayOfIndices = new int[items.Count];

            for (int i = 0; i < arrayOfIndices.Length; i++)
            {
                arrayOfIndices[i] = i;
            }

            indicesOfChosenItems = ChooseRandomNElementsFromArray<int>(arrayOfIndices, numberOfHiddenItems);
            isFirstTimeDone = true;
        }

        if(indexOfIndicesOfChosenItems < indicesOfChosenItems.Length)
        {
            currentItemIndex = indicesOfChosenItems[indexOfIndicesOfChosenItems++];
            print(currentItemIndex);
        }
        else
        {
            OnChallengeFinished.Invoke();
            return;
        }

        ActivateCurrentText();
    }

    void CheckForCorrectItem(ItemBehaviour item)
    {
        if(item.GetInstanceID() == items[currentItemIndex].GetInstanceID())
        {
            audio.PlayOneShot(correct_sfx);
            StartCoroutine(item.Flash(duration, frequency, flashColor, ChooseRandomObject));
        }
    }

    private void OnEnable()
    {
        indexOfIndicesOfChosenItems = 0;

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
        isFirstTimeDone = false;
        indexOfIndicesOfChosenItems = 0;
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
        texts[currentItemIndex].SetActive(true);
    }

    /// <summary>
    /// This method picks n Random elements from the array and returns an array with those elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    private T[] ChooseRandomNElementsFromArray<T>(T[] array, int n)
    {
        var randomObjects = new T[array.Length];

        for (int i = 0; i < n; i++)
        {
            // Take only from the latter part of the list - ignore the first i items.
            int take = UnityEngine.Random.Range(i, array.Length);
            randomObjects[i] = array[take];

            // Swap our random choice to the beginning of the array,
            // so we don't choose it again on subsequent iterations.
            array[take] = array[i];
            array[i] = randomObjects[i];
        }

        T[] output = new T[n];

        Array.Copy(randomObjects, 0, output, 0, output.Length);

        return output;
    }
}
