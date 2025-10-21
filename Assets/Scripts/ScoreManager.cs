//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI tb;
    [SerializeField] private GameOverChecker goc;
    private static int score = 0;
    private static bool needToUpdate = false;

    public static void increseScore(int x)
    {
        score += x;
        needToUpdate = true;
        
    }


    void Start()
    {
        tb.text = "Score: " + score;
    }

    void Update()
    {
        goc.score = score;
        if(needToUpdate){
            tb.text = "Score: " + score;
        }
    }
}
