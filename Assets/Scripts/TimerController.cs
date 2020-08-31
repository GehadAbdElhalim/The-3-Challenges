using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerController : MonoBehaviour
{
    #region Singleton
    public static TimerController Instance { private set; get; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private float timerSeconds, timerMins, timerHours;
    public UnityEvent OnTimerEnd = new UnityEvent();
    public OnTimerValueChanged onTimerValueChanged = new OnTimerValueChanged();
    private bool isTimerOn;
    private int prevSecond;

    public float secondsElapsed = 0;

    public string currentTime;

    public void StartTimer(float seconds, UnityAction<string> timerValueChanged, UnityAction endTimerCallBack)
    {
        timerSeconds = seconds;

        //OnTimerEnd = new UnityEvent();
        OnTimerEnd.AddListener(endTimerCallBack);

        //onTimerValueChanged = new OnTimerValueChanged();
        onTimerValueChanged.AddListener(timerValueChanged);

        prevSecond = (int)timerSeconds;

        isTimerOn = true;
    }

    public void StartTimer(float seconds, float minutes, UnityAction<string> timerValueChanged, UnityAction endTimerCallBack)
    {
        timerSeconds = seconds;
        timerMins = minutes;

        //OnTimerEnd = new UnityEvent();
        if (endTimerCallBack != null)
        {
            OnTimerEnd.AddListener(endTimerCallBack);
        }

        //onTimerValueChanged = new OnTimerValueChanged();
        if (timerValueChanged != null)
        {
            onTimerValueChanged.AddListener(timerValueChanged);
        }

        prevSecond = (int)timerSeconds;

        isTimerOn = true;
    }

    public void StartTimer(float seconds, float minutes, float hours, UnityAction<string> timerValueChanged, UnityAction endTimerCallBack)
    {
        timerSeconds = seconds;
        timerMins = minutes;
        timerHours = hours;

        //OnTimerEnd = new UnityEvent();
        OnTimerEnd.AddListener(endTimerCallBack);

        //onTimerValueChanged = new OnTimerValueChanged();
        onTimerValueChanged.AddListener(timerValueChanged);

        prevSecond = (int)timerSeconds;

        isTimerOn = true;
    }

    public void StartCountUpTimer(UnityAction<string> timerValueChanged)
    {
        timerSeconds = 0;
        timerMins = 0;
        timerHours = 0;
        secondsElapsed = 0;

        //onTimerValueChanged = new OnTimerValueChanged();
        onTimerValueChanged.AddListener(timerValueChanged);

        prevSecond = (int)timerSeconds;

        isTimerOn = true;
    }

    void FixedUpdate()
    {
        if (isTimerOn)
        {
            if (timerSeconds > 0 || timerMins > 0 || timerHours > 0)
            {
                timerSeconds -= Time.deltaTime;

                if (timerSeconds < 0)
                {
                    if (timerMins > 0)
                    {
                        timerMins--;
                        timerSeconds = 59f;
                    }
                    else if (timerHours > 0)
                    {
                        timerHours--;
                        timerMins = 59f;
                        timerSeconds = 59f;
                    }
                }

                string time = ((timerHours > 0) ? timerHours.ToString("00") + ":" : "") + (timerMins.ToString("00") + ":" ) + timerSeconds.ToString("00");
                onTimerValueChanged.Invoke(time);
            }
            else
            {
                isTimerOn = false;
                OnTimerEnd.Invoke();
            }
        }
    }

    public void ResetTimer()
    {
        timerSeconds = 0;
        timerMins = 0;
        timerHours = 0;
        secondsElapsed = 0;
    }
}

[System.Serializable]
public class OnTimerValueChanged : UnityEvent<string>
{
}