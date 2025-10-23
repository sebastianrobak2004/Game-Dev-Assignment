using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreKeeper : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI tb;

    void Start()
    {
        
    }

    void Update()
    {
        tb.text = "HighScore: " + PlayerPrefs.GetInt("HighScore", 0);
    }
}
