using UnityEngine;
using TMPro;

public class TimeLeftScript : MonoBehaviour
{
    TMP_Text text;
    public static float timeLeft = 30f;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) timeLeft = 0;

        text.text = "Time left: " + Mathf.Round(timeLeft);
    }
}
