using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public string[] scenes;


    private void Start()
    {
        SceneManager.LoadScene("MainMenu"); //The SceneController only starts in the setup scene so we can just start it right away
    }

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

    public void loadEmptyScene()
    {
        SceneManager.LoadScene(scenes[scenes.Length - 1]);
    }

    public bool playableScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if(activeScene.name == "MainMenu" || activeScene.name == "Setup" || activeScene.name == scenes[scenes.Length - 1]) //Last Scene in List is EmptyScene
        {
            return false;
        }
        return true;
    }
}
