using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RealStartAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tb;
    private string ani;
    private int i;
    [SerializeField] private int length;

    void Start()
    {
      
        StartCoroutine(AnimateEveryHalfSecond());
    }

    private IEnumerator AnimateEveryHalfSecond()
    {
        while (true)
        {
            if(i < length)
            {
                ani += ". ";
                i+=1;
            }
            else
            {
                ani = "";
                i = 0;
            }
            
            tb.text = ani;
            
            yield return new WaitForSeconds(0.5f);
        }
    }
}