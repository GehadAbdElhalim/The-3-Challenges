using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FTDChallenge : Challenge
{
    AudioSource audio;

    private int progress;

    [Header("Duration of the challenge")]
    public int minutes;
    public int seconds;

    List<DifferenceBehaviour> _leftSideDifferences = new List<DifferenceBehaviour>();
    List<DifferenceBehaviour> _rightSideDifferences = new List<DifferenceBehaviour>();

    public AudioClip correct_sfx;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        progress = 0;

        //Get all the differences
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            _leftSideDifferences.Add(transform.GetChild(0).GetChild(i).GetComponent<DifferenceBehaviour>());
            _rightSideDifferences.Add(transform.GetChild(1).GetChild(i).GetComponent<DifferenceBehaviour>());

            //Assign index
            _leftSideDifferences[i].index = i;
            _rightSideDifferences[i].index = i;
        }


        //Listen to all Differences' events
        foreach(var difference in _leftSideDifferences)
        {
            difference.OnChecked.AddListener(OnDifferenceFound);
            difference.OnCheckedFinished.AddListener(IncreaseProgress);
        }

        foreach(var difference in _rightSideDifferences)
        {
            difference.OnChecked.AddListener(OnDifferenceFound);
            difference.OnCheckedFinished.AddListener(IncreaseProgress);
        }
    }

    private void OnEnable()
    {
        if (!ChallengeManager.Instance.tutorial.activeSelf)
        {
            TimerController.Instance.StartTimer(seconds, minutes, null, null);
        }
    }

    void OnDifferenceFound(int index)
    {
        audio.PlayOneShot(correct_sfx);

        _leftSideDifferences[index].Activate();
        _rightSideDifferences[index].Activate();
    }

    public void IncreaseProgress()
    {
        progress++;

        if(progress >= _leftSideDifferences.Count)
        {
            OnChallengeFinished.Invoke();
        }
    }

    public override void ResetProgress()
    {
        progress = 0;

        foreach (var difference in _leftSideDifferences)
        {
            difference.DeActivate();
        }

        foreach (var difference in _rightSideDifferences)
        {
            difference.DeActivate();
        }

        TimerController.Instance.StartTimer(seconds, minutes, null, null);
    }

    public List<DifferenceBehaviour> GetLeftSideDifferences()
    {
        return _leftSideDifferences;
    }

    public List<DifferenceBehaviour> GetRightSideDifferences()
    {
        return _rightSideDifferences;
    }
}
