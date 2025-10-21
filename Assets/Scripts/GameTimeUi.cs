using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour

{

    [SerializeField] private TextMeshProUGUI timerText;
    
    private float elapsed = 0f;
    private bool running = true;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTimerText(0f);
    }

    void Update()
    {
        if (!running) return;

        elapsed += Time.deltaTime;
        UpdateTimerText(elapsed);
    }

    private void UpdateTimerText(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);

        if (timerText != null)
        {
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
