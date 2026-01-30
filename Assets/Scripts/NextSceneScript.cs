using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NextSceneScript : MonoBehaviour
{
    public string SceneName;
    public void OnStartClick()
    {
        SceneManager.LoadScene(SceneName);
    }

   
}

