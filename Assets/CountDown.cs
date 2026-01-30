using UnityEngine;
using TMPro;
using System;
public class CountDown : MonoBehaviour
{
    public static CountDown instance;
    [SerializeField] private TextMeshProUGUI timerText;
    public float currentTime = 30f;
    private bool active = true;

   

       void Update()
    {
        if (!active)
        return;
        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        if(currentTime <= 0)
        {
            StopTimer();
        }


        
    }
    public void StopTimer()
    {
        active = false;
        currentTime = 0f;
        UpdateTimerUI();
        
    }

    private void UpdateTimerUI()
    {
        if(currentTime > 0 && currentTime < 6)
        {
            timerText.color = Color.yellow;
        }
        else if (currentTime < 1)
        {
            timerText.color = Color.red;
        }

        TimeSpan t = TimeSpan.FromSeconds(currentTime);
        timerText.text = t.ToString(@"m\:s");

    }

    public void IncreaseCoins(int v)
    {
        currentTime += v;
    }

    void Awake()
    {
        instance = this;
    }


}
