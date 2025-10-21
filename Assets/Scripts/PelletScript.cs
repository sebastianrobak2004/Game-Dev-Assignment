using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletScript : MonoBehaviour
{
    [SerializeField]private ScoreManager scoreManager;
    private PelletTilemapSpawner pelletManager;
    [SerializeField]private int ScoreIncreaseFromNormalPellet;
    
    void Start()
    {
        pelletManager = FindFirstObjectByType<PelletTilemapSpawner>();
        if (scoreManager == null)
            scoreManager = FindFirstObjectByType<ScoreManager>();
    }


    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {   
            if(gameObject.tag == "PowerPellet"){
                pelletManager.PelletEaten();
            }
            
            ScoreManager.increseScore(ScoreIncreaseFromNormalPellet);
            Destroy(gameObject);        
        }

    }
}
