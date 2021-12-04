using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public string[] scenes;


    public void loadNextScene()
    {
        if(scenes != null)
        {

            for(int i = 0; i < scenes.Length; i++)
            {
                if(scenes[i] == SceneManager.GetActiveScene().name)
                {
                    if(scenes.Length > i) //Have to check this in order to make sure were not at the last level 
                    {
                        SceneManager.LoadScene(scenes[i + 1]);
                        return;
                    }
                    Debug.LogError("SceneController trying to load scene over scene array boundary");
                }
            }
            Debug.LogError("Active scene not in SceneControllerArray");
        }
    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
