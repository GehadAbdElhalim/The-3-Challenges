using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public float secondsDeductedForHint;

    public void ShowHint()
    {
        Challenge c = ChallengeManager.Instance.GetCurrentChallenge();

        if(c != null && c is FTDChallenge)
        {
            List<DifferenceBehaviour> left = ((FTDChallenge)c).GetLeftSideDifferences();
            List<DifferenceBehaviour> right = ((FTDChallenge)c).GetRightSideDifferences();

            for (int i = 0; i < left.Count; i++)
            {
                if (!left[i].IsAlreadyActive())
                {
                    left[i].ShowHint();
                    right[i].ShowHint();
                    TimerController.Instance.DecreaseTime(secondsDeductedForHint);
                    return;
                }
            }
        }
    }
}
