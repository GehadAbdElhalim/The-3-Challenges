﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Challenge : MonoBehaviour
{
    public UnityEvent OnChallengeFinished;

    public virtual void ResetProgress()
    {

    }
}