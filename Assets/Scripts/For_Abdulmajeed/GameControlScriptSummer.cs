using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlScriptSummer : MonoBehaviour
{
    public GameObject timeIsUp,restartButtonForTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeLeftScript.timeLeft <= 0)
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
        SceneManager.LoadScene("SummerEdition");
    }
}
