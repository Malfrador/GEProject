using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepController : MonoBehaviour
{

    private ArrayList positions = new ArrayList();

    public bool registerPosition(Vector3Int newPosition)
    {
        foreach(Vector3Int pos in positions)
        {
            if(pos == newPosition)
            {
                return false;
            }
        }

        positions.Add(newPosition);
        return true;
    }

    public void unregisterOldPosition(Vector3Int oldPosition)
    {
        positions.Remove(oldPosition);
    }
}
