using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChallengeManager : MonoBehaviour
{
    #region singleton
    public static ChallengeManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public ChallengeType ChallengeType;
    public GameObject challengeContainer;
    public bool randomizeOrderOfChallenges;

    int progress = 0;

    List<Challenge> _challenges = new List<Challenge>();

    private void Start()
    {
        if (randomizeOrderOfChallenges)
        {
            ShuffleChallenges();
        }

        //Get all the challenges
        for(int i = 0; i < challengeContainer.transform.childCount; i++)
        {
            _challenges.Add(challengeContainer.transform.GetChild(i).GetComponent<Challenge>());
        }

        //Listen to their events
        foreach(var challenge in _challenges)
        {
            challenge.OnChallengeFinished.AddListener(OnChallengeFinished);
        }

        DisableAllChallenges();
        EnableChallenge(progress);
    }

    void ShuffleChallenges()
    {

    }

    void DisableAllChallenges()
    {
        foreach(var challenge in _challenges)
        {
            challenge.gameObject.SetActive(false);
        }
    }

    void EnableChallenge(int index)
    {
        _challenges[index].gameObject.SetActive(true);
    }

    public void OnChallengeFinished()
    {
        progress++;

        if(progress >= _challenges.Count)
        {
            //All the challenges are finished
            print("Done");
        }
        else
        {
            DisableAllChallenges();
            EnableChallenge(progress);
        }
    }

    Challenge GetCurrentChallenge()
    {
        foreach(Challenge c in _challenges)
        {
            if (c.gameObject.activeSelf)
            {
                return c;
            }
        }

        return null;
    }

    public void RestartCurrentChallenge()
    {
        GetCurrentChallenge().ResetProgress();
    }
}

public enum ChallengeType
{
    Find_The_Differences = 0,
    Find_Hidden_Items = 1
}
