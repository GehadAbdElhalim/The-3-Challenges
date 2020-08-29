using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTDChallenge : MonoBehaviour
{
    public int numberOfDifferences;
    public int progress;
    private void Start()
    {
        numberOfDifferences = transform.GetChild(0).childCount;
    }
}
