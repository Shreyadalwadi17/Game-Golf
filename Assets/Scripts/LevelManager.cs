using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> levels;
    public ScoreBoard scoreBoard;
    public BallController ballController;

    private void Start()
    {
        LevelCheck();
        scoreBoard.clubText.text = scoreBoard.clubValue.ToString();
    }
    public void LevelCheck()
    {
       
        switch (ballController.currentLevel)
        {
            case 1:
                scoreBoard.clubValue = 3;
                scoreBoard.clubText.text = scoreBoard.clubValue.ToString();
                break;

            case 2:
                scoreBoard.clubValue = 4;
                scoreBoard.clubText.text = scoreBoard.clubValue.ToString();
                break;
            case 3:
                scoreBoard.clubValue = 6;
                scoreBoard.clubText.text = scoreBoard.clubValue.ToString();
                break;

        }
    }
}


    

