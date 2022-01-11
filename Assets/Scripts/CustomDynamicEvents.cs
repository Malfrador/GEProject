using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDynamicEvents : MonoBehaviour
{
    public void setActiveTrue(string gameobject)
    {
        Transform[] alls = Resources.FindObjectsOfTypeAll<Transform>();
        foreach(Transform trans in alls)
        {
            if(trans.name == gameobject)
            {
                trans.gameObject.SetActive(true);
            }
        }
    }
}
