using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FTDManager : MonoBehaviour
{
    public ChallengeType ChallengeType;
    public GameObject challengeContainer;
    public bool randomizeOrderOfChallenges;
    int progress = 0;

    private void Start()
    {
        if (randomizeOrderOfChallenges)
        {
            ShuffleChallenges();
        }

        DisableAllChallenges();
        EnableChallenge(progress);
    }

    void ShuffleChallenges()
    {

    }

    void DisableAllChallenges()
    {
        for(int i = 0; i < challengeContainer.transform.childCount; i++)
        {
            challengeContainer.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void EnableChallenge(int index)
    {
        challengeContainer.transform.GetChild(index).gameObject.SetActive(true);
    }

    public void OnChallengeFinished()
    {
        progress++;

        if(progress >= challengeContainer.transform.childCount)
        {
            //All the challenges are finished
        }
        else
        {
            DisableAllChallenges();
            EnableChallenge(progress);
        }
    }
}

public enum ChallengeType
{
    Find_The_Differences = 0,
    Find_Hidden_Items = 1
}
