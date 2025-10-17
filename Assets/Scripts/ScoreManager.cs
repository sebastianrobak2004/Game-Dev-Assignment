using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI tb;
    private static int score = 0;
    private static bool needToUpdate = false;
    // Start is called before the first frame update
    public static void increseScore(int x)
    {
        score += x;
        needToUpdate = true;
    }


    void Start()
    {
        tb.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        if(needToUpdate){
            tb.text = "Score: " + score;
        }
    }
}
