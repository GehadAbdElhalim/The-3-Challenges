using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RTLTMPro;

public class FTDChallenge : Challenge
{
    AudioSource audio;

    private int progress;

    [Header("Specific parameters for this challenge")]
    [SerializeField] RTLTextMeshPro RemaningText;

    [SerializeField] CanvasGroup image1;
    [SerializeField] CanvasGroup image2;

    [Header("Duration of the challenge")]
    public int minutes;
    public int seconds;

    List<DifferenceBehaviour> _leftSideDifferences = new List<DifferenceBehaviour>();
    List<DifferenceBehaviour> _rightSideDifferences = new List<DifferenceBehaviour>();

    [Header("SFX")]
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

        int remaning = _leftSideDifferences.Count - progress;

        RemaningText.text = remaning.ToString() + " اختلافات متبقية";
    }

    private void OnEnable()
    {
        int remaning = _leftSideDifferences.Count - progress;

        RemaningText.text = remaning.ToString() + " اختلافات متبقية";

        image1.alpha = 1;
        image2.alpha = 0;

        Invoke("StartTimer", Time.deltaTime);
    }

    void StartTimer()
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

        int remaning = _leftSideDifferences.Count - progress;

        RemaningText.text = remaning.ToString() + " اختلافات متبقية";

        if(progress >= _leftSideDifferences.Count)
        {
            FinishChallengeWithDelay();
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

    public void SwitchImages()
    {
        if(image1.alpha == 0)
        {
            image1.alpha = 1;
            image2.alpha = 0;
        }
        else
        {
            image1.alpha = 0;
            image2.alpha = 1;
        }
    }
}
