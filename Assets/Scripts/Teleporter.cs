using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    [SerializeField] private PacmanMovement pacman;
    [SerializeField] private Vector2 linkedLocation;
    



    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            
            if (pacman != null)
            {
                // Reset Pac-Man completely at the new position
                pacman.ResetAtPosition(linkedLocation);
                
            }

        }
    }


}
