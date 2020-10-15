using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerController : MonoBehaviour
{
    #region Singleton

    private static TimerController _instance;
    public static TimerController Instance {
        private set
        {
            _instance = FindObjectOfType<TimerController>();
        }
        get 
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<TimerController>();
            }

            return _instance;
        
        } }


    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
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

    public string currentTime;

    [Header("starting values")]
    public float startingSeconds;
    public float startingMinutes;
    public float startingHours;

    public void StartTimer(float seconds, UnityAction<string> timerValueChanged, UnityAction endTimerCallBack)
    {
        timerSeconds = seconds;

        //OnTimerEnd = new UnityEvent();
        OnTimerEnd.AddListener(endTimerCallBack);

        //onTimerValueChanged = new OnTimerValueChanged();
        onTimerValueChanged.AddListener(timerValueChanged);

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

        isTimerOn = true;
    }

    void OnEnable()
    {
        timerHours = startingHours;
        timerMins = startingMinutes;
        timerSeconds = startingSeconds;

        string time = ((timerHours > 0) ? timerHours.ToString("00") + ":" : "") + (timerMins.ToString("00") + ":") + timerSeconds.ToString("00");
        onTimerValueChanged.Invoke(time);   
    }

    void Update()
    {
        if (isTimerOn)
        {
            DecreaseTime(Time.deltaTime);
        }
    }

    public void DecreaseTime(float seconds)
    {
        if (timerSeconds > 0 || timerMins > 0 || timerHours > 0)
        {
            timerSeconds -= seconds;

            if (timerSeconds < 0)
            {
                if (timerMins > 0)
                {
                    timerMins--;
                    timerSeconds = 59f + timerSeconds;
                }
                else if (timerHours > 0)
                {
                    timerHours--;
                    timerMins = 59f;
                    timerSeconds = 59f + timerSeconds;
                }
                else
                {
                    timerSeconds = 0f;
                }
            }

            string time = ((timerHours > 0) ? timerHours.ToString("00") + ":" : "") + (timerMins.ToString("00") + ":") + timerSeconds.ToString("00");
            onTimerValueChanged.Invoke(time);
        }
        else
        {
            isTimerOn = false;
            OnTimerEnd.Invoke();
        }
    }

    public void ResetTimer()
    {
        timerSeconds = 0;
        timerMins = 0;
        timerHours = 0;
    }

    public void PauseTimer()
    {
        isTimerOn = false;
    }

    public void UnpauseTimer()
    {
        isTimerOn = true;
    }
}

[System.Serializable]
public class OnTimerValueChanged : UnityEvent<string>
{
}