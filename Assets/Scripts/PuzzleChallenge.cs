using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleChallenge : Challenge
{
    public Image puzzlePiecePrefab;
    public Transform parent;

    private void Start()
    {
        //MakeGrid(new Vector3(-566,295,0));
    }

    void MakeGrid(Vector3 startPos)
    {
        for(int j = 0; j < 5; j++)
        {
            for(int i = 0; i < 5; i++)
            {
                Instantiate(puzzlePiecePrefab, new Vector3(startPos.x + i * 100, startPos.y - j * 100, 0), Quaternion.identity, parent);
            }
        }
    }

    public override void ResetProgress()
    {
        
    }
}
