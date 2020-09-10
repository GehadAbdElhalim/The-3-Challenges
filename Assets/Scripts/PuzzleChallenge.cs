using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleChallenge : Challenge
{
    public GameObject Father;
    public GameObject Son;

    public GameObject[] FatherPuzzleItems;
    public GameObject[] SonPuzzleItems;

    public EmptyCell[] FatherEmptyCells;
    public EmptyCell[] SonEmptyCells;

    private bool[] CorrectPuzzlePieces = new bool[50];

    [Header("Duration of the challenge")]
    public int minutes;
    public int seconds;

    private void Start()
    {
        foreach(EmptyCell ec in FatherEmptyCells)
        {
            ec.OnPiecePut.AddListener(CheckPuzzle);
        }

        foreach (EmptyCell ec in SonEmptyCells)
        {
            ec.OnPiecePut.AddListener(CheckPuzzle);
        }
    }

    void CheckPuzzle(int cellIndex, int puzzlePieceIndex)
    {
        if(cellIndex == puzzlePieceIndex)
        {
            CorrectPuzzlePieces[cellIndex] = true;
            if (CheckIFPuzzleIsComplete())
            {
                OnChallengeFinished.Invoke();
                return;
            }
        }

        Invoke("SwitchTurn", 1f);
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

    void RandomizePuzzleItems(GameObject[] arr)
    {
        for (int t = 0; t < arr.Length; t++)
        {
            Vector3 tmp = arr[t].transform.position;
            int r = Random.Range(t, arr.Length);
            arr[t].transform.position = arr[r].transform.position;
            arr[r].transform.position = tmp;
        }
    }

    private void OnEnable()
    {
        TimerController.Instance.StartTimer(seconds, minutes, null, null);
        RandomizePuzzleItems(FatherPuzzleItems);
        RandomizePuzzleItems(SonPuzzleItems);
        Father.SetActive(true);
        Son.SetActive(false);
    }

    public void SwitchTurn()
    {
        Father.SetActive(!Father.activeSelf);
        Son.SetActive(!Son.activeSelf);
    }

    public override void ResetProgress()
    {
        TimerController.Instance.StartTimer(seconds, minutes, null, null);
    }
}
