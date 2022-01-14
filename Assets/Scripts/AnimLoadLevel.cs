using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLoadLevel : MonoBehaviour
{
    private string levelName;
    public SceneController sceneController;
    public Animator animator;

    public void loadLevelWithAnim(string name)
    {
        levelName = name;
        animator.Play("BlendAnimation");
    }
    public void AnimOver()
    {
        sceneController.loadScene(levelName);
    }
}
