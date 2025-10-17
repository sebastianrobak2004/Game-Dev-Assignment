using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTracker : MonoBehaviour

{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> HeartSprites;

    private int lives = 3;
    private bool needUpdate = false;
    // Start is called before the first frame update
    public void TookDamage()
    {
        lives -= 1;
        needUpdate = true;
    }


    // Update is called once per frame
    void Update()
    {
        if(needUpdate)
        {
            spriteRenderer.sprite = HeartSprites[lives-1];
        }

        //test
        if(Input.GetKeyDown(KeyCode.Space)){
            TookDamage();
        }
        
    }
}
