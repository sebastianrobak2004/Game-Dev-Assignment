using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreKeeper : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]private TextMeshProUGUI tb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tb.text = "HighScore: " + PlayerPrefs.GetInt("HighScore", 0);
    }
}
