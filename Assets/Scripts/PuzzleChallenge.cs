﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleChallenge : Challenge
{
    AudioSource audio;

    [Header("Specific parameters for this challenge")]
    public GameObject FullImage;

    public GameObject EmptyCellsParent;
    public GameObject Father;
    public GameObject Son;

    public GameObject FatherTxt;
    public GameObject SonTxt;

    //public GameObject blockInput;
    
    private PuzzleItem[] FatherPuzzleItems;
    private PuzzleItem[] SonPuzzleItems;

    private EmptyCell[] FatherEmptyCells;
    private EmptyCell[] SonEmptyCells;

    private bool[] CorrectPuzzlePieces = new bool[50];

    [Header("Duration of the challenge")]
    public int minutes;
    public int seconds;

    [Header("SFX")]
    public AudioClip PuzzlePiecePickedUp;
    public AudioClip PuzzlePieceInsertedInCell;

    private void Awake()
    {
        FatherPuzzleItems = Father.GetComponentsInChildren<PuzzleItem>();
        SonPuzzleItems = Son.GetComponentsInChildren<PuzzleItem>();

        FatherEmptyCells = EmptyCellsParent.transform.GetChild(0).GetComponentsInChildren<EmptyCell>();
        SonEmptyCells = EmptyCellsParent.transform.GetChild(1).GetComponentsInChildren<EmptyCell>();
    }

    private void Start()
    {
        audio = transform.parent.GetComponent<AudioSource>();

        foreach (EmptyCell ec in FatherEmptyCells)
        {
            ec.OnPiecePut.AddListener(CheckPuzzle);
        }

        foreach (EmptyCell ec in SonEmptyCells)
        {
            ec.OnPiecePut.AddListener(CheckPuzzle);
        }

        foreach(PuzzleItem pi in FatherPuzzleItems)
        {
            pi.OnPieceThrown.AddListener(OnPieceDroppedOutside);
            pi.OnPiecePickedUp.AddListener(PlayPickupSFX);
        }

        foreach (PuzzleItem pi in SonPuzzleItems)
        {
            pi.OnPieceThrown.AddListener(OnPieceDroppedOutside);
            pi.OnPiecePickedUp.AddListener(PlayPickupSFX);
        }
    }

    void CheckPuzzle(int cellIndex, int puzzlePieceIndex)
    {
        //SFX
        PlayInsertSFX();

        //logic
        if(cellIndex == puzzlePieceIndex)
        {
            if (CorrectPuzzlePieces[cellIndex])
            {
                return;
            }

            CorrectPuzzlePieces[cellIndex] = true;

            if (CheckIFPuzzleIsComplete())
            {
                FinishChallengeWithDelay();
                return;
            }
        }

        //blockInput.SetActive(true);
        SwitchTurn();
    }

    void OnPieceDroppedOutside(int index)
    {
        CorrectPuzzlePieces[index] = false;
    }

    void PlayPickupSFX()
    {
        audio.PlayOneShot(PuzzlePiecePickedUp);
    }

    void PlayInsertSFX()
    {
        audio.PlayOneShot(PuzzlePieceInsertedInCell);
    }

    bool CheckIFPuzzleIsComplete()
    {
        foreach(bool b in CorrectPuzzlePieces)
        {
            if (!b)
            {
                return false;
            }
        }

        return true;
    }

    void RandomizePuzzleItems(PuzzleItem[] arr)
    {
        for (int t = 0; t < arr.Length; t++)
        {
            Vector3 tmp = arr[t].transform.position;
            int r = Random.Range(t, arr.Length);
            arr[t].transform.position = arr[r].transform.position;
            arr[r].transform.position = tmp;
        }

        foreach(PuzzleItem pi in arr)
        {
            pi.SetOriginalPosition();
        }
    }

    private void OnEnable()
    {
        RandomizePuzzleItems(FatherPuzzleItems);
        RandomizePuzzleItems(SonPuzzleItems);

        FatherTxt.SetActive(true);
        SonTxt.SetActive(false);
        FullImage.SetActive(false);
        //blockInput.SetActive(false);

        Invoke("StartTimer", Time.deltaTime);
    }

    void StartTimer()
    {
        if (!ChallengeManager.Instance.tutorial.activeSelf)
        {
            TimerController.Instance.StartTimer(seconds, minutes, null, null);
        }
    }

    public void SwitchTurn()
    {
        FatherTxt.SetActive(!FatherTxt.activeSelf);
        SonTxt.SetActive(!SonTxt.activeSelf);

        //blockInput.SetActive(false);
    }

    public override void ResetProgress()
    {
        foreach(PuzzleItem pi in FatherPuzzleItems)
        {
            pi.ResetToOriginalPosition();
        }
        
        foreach(PuzzleItem pi in SonPuzzleItems)
        {
            pi.ResetToOriginalPosition();
        }

        CorrectPuzzlePieces = new bool[50];

        TimerController.Instance.StartTimer(seconds, minutes, null, null);
    }

    public void ToggleFullImage()
    {
        FullImage.SetActive(!FullImage.activeSelf);
        //blockInput.SetActive(FullImage.activeSelf);
    }

    [ContextMenu("Finish Challenge")]
    void FinishChallenge()
    {
        FinishChallengeWithDelay();
    }
}
