using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CustomUtil
{
    static public Vector3Int Vector3ToInt(Vector3 vector3)
    {
        return new Vector3Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
    }

    static public char angleToLookDir(int angle)
    {
        switch(angle)
        {
            case 0:
                return 'E';
            case 90:
                return 'N';
            case 180:
                return 'W';
            case 270:
                return 'S';
        }
        return 'E';
    }
}
