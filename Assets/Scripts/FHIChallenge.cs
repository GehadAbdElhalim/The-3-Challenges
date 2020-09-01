using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FHIChallenge : Challenge
{
    public OnItemChosenEvent OnItemChosen = new OnItemChosenEvent();

    public List<ItemBehaviour> items;
    public List<string> texts;

    [Header("Duration of the challenge")]
    public int minutes;
    public int seconds;

    ItemBehaviour currentItem;
    string currentText;

    void Start()
    {
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
            OnChallengeFinished.Invoke();
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

public class OnItemChosenEvent : UnityEvent<string>
{

}
