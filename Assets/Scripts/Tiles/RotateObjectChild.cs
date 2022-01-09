using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectChild : MonoBehaviour
{
    public RotateObject rotateObject;
    private void OnMouseOver()
    {
        rotateObject.OnMouseOverReroute();
    }
}
