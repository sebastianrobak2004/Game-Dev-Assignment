using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletScript : MonoBehaviour
{
    [SerializeField]private ScoreManager scoreManager;
    [SerializeField]private int ScoreIncreaseFromNormalPellet;
    
    // Start is called before the first frame update
    void Start()
    {
        if (scoreManager == null)
            scoreManager = FindFirstObjectByType<ScoreManager>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            ScoreManager.increseScore(ScoreIncreaseFromNormalPellet);
            Destroy(gameObject);        
        }

    }
}
