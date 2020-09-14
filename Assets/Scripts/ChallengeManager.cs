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

    private void OnEnable()
    {
        if (!Instance)
        {
            Instance = this;
        }

        switch (challengeType)
        {
            case ChallengeType.Puzzle:
                if (PlayerPrefs.GetInt("Puzzle") == 0)
                {
                    PlayerPrefs.SetInt("Puzzle", 1);
                    tutorial?.SetActive(true);
                }
                break;
            case ChallengeType.Find_the_differences:
                if (PlayerPrefs.GetInt("FTD") == 0)
                {
                    PlayerPrefs.SetInt("FTD", 1);
                    tutorial?.SetActive(true);
                }
                break;
            case ChallengeType.Find_the_hidden_items:
                if (PlayerPrefs.GetInt("FHI") == 0)
                {
                    PlayerPrefs.SetInt("FHI", 1);
                    tutorial?.SetActive(true);
                }
                break;
            default:
                break;
        }
    }
    #endregion

    public UnityEvent OnLevelCompleted = new UnityEvent();

    public ChallengeType challengeType;

    public GameObject challengeContainer;

    public GameObject tutorial;

    int progress = 0;

    List<Challenge> _challenges = new List<Challenge>();

    [ContextMenu("Clear Data")]
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        //Get all the challenges
        for (int i = 0; i < challengeContainer.transform.childCount; i++)
        {
            _challenges.Add(challengeContainer.transform.GetChild(i).GetComponent<Challenge>());
        }

        //Listen to their events
        foreach (var challenge in _challenges)
        {
            challenge.OnChallengeFinished.AddListener(OnChallengeFinished);
        }

        DisableAllChallenges();
        EnableChallenge(progress);
    }

    void DisableAllChallenges()
    {
        foreach (var challenge in _challenges)
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

        if (progress >= _challenges.Count)
        {
            OnLevelCompleted.Invoke();
        }
        else
        {
            DisableAllChallenges();
            EnableChallenge(progress);
        }
    }

    public Challenge GetCurrentChallenge()
    {
        return progress < _challenges.Count ? _challenges[progress] : null;
    }

    public void RestartCurrentChallenge()
    {
        GetCurrentChallenge()?.ResetProgress();
    }
}

public enum ChallengeType
{
    Puzzle = 0,
    Find_the_differences = 1,
    Find_the_hidden_items = 2
}