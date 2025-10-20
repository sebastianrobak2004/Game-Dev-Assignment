using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverChecker : MonoBehaviour
{
    [SerializeField] private PacmanMovement pacman;
    [SerializeField]private TextMeshProUGUI tb;
    [SerializeField] public int score;
    void Update()
    {
        if (pacman.dead || score == 2300)
        {
            tb.text = "GAME OVER! \n Score is: " + score;

            if(score > PlayerPrefs.GetInt("HighScore", 0)){
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            }
        }
        
    }
}
