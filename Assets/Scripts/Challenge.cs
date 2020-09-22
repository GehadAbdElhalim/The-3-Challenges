using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Challenge : MonoBehaviour
{
    //Used for adding effects before the challenge end
    public UnityEvent OnChallengeAboutToFinish;

    [HideInInspector] public UnityEvent OnChallengeFinished;

    public float timeBeforeChallengeIsFinished;

    public virtual void ResetProgress()
    {

    }

    public void FinishChallengeWithDelay()
    {
        OnChallengeAboutToFinish.Invoke();
        Invoke("EndChallenge", timeBeforeChallengeIsFinished);
    }

    void EndChallenge()
    {
        OnChallengeFinished.Invoke();
    }
}
