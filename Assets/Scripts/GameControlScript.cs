using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject restartButton;

    void Start()
    {
        restartButton.SetActive(false);   // hide at start
    }

    void Update()
    {
        if (Player == null)
        {
            restartButton.SetActive(true);
        }
    }

    public void restartScene()
    {
        restartButton.SetActive(false);
        SceneManager.LoadScene("Winter_Season");
    }
}
