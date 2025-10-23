using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTracker : MonoBehaviour

{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> HeartSprites;
    [SerializeField] private AudioSource bonkSound;

    private int lives = 3;
    private bool needUpdate = false;
    public void TookDamage()
    {
        lives -= 1;
        needUpdate = true;
        bonkSound.Play();
    }


    void Update()
    {
        if(needUpdate & lives > 0)
        {
            spriteRenderer.sprite = HeartSprites[lives-1];
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            TookDamage();
        }
        
    }
}
