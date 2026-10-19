using UnityEngine;
using UnityEngine.SceneManagement;

// Summer level: freezes the game and shows the restart UI when the timer runs out.
public class GameControlScriptSummer : MonoBehaviour
{
    public GameObject timeIsUp, restartButtonForTimer;

    void Update()
    {
        if (TimeLeftScript.timeLeft <= 0)
        {
            Time.timeScale = 0;
            timeIsUp.gameObject.SetActive(true);
            restartButtonForTimer.gameObject.SetActive(true);
        }
    }

    public void restartSceneForSummer()
    {
        timeIsUp.gameObject.SetActive(false);
        restartButtonForTimer.gameObject.SetActive(false);
        Time.timeScale = 1;
        TimeLeftScript.timeLeft = 30f;
        SceneManager.LoadScene("Summer");
    }
}
