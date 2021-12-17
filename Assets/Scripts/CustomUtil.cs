using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CustomUtil
{
    static public Vector3Int Vector3ToInt(Vector3 vector3)
    {
        return new Vector3Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
    }
}
