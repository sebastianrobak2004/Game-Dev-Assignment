using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverChecker : MonoBehaviour
{
    [SerializeField] private PacmanMovement pacman;
    [SerializeField]private TextMeshProUGUI tb;
    [SerializeField] public int score;

    public bool gameHasEnded => pacman.dead & score == 2300;
    public bool newHighScore => score > PlayerPrefs.GetInt("HighScore", 0);

    void Update()
    {
        if (gameHasEnded)
        {
            tb.text = "GAME OVER! \n Score is: " + score;

            if(newHighScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
                PlayerPrefs.Save();
            }
        }
        
    }
}
