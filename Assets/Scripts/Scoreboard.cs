using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public int scoreLimit = 100;
    private int curScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject puzzle = GameObject.Find("Puzzle");
            puzzle.transform.position = Vector3.zero;
            puzzle.BroadcastMessage("StartGame");
        }
    }

    public void IncrementScore()
    {
        ++curScore;
        // Update ui score
        if (curScore == scoreLimit)
        {
            //TODO: game over - victory
        }
    }
}
