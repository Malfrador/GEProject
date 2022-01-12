using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepController : MonoBehaviour
{

    public struct registeredPeep
    {
        public Peep peep;
        public Vector3Int peepPos;
    }


    private ArrayList positions = new ArrayList();

    public bool registerPosition(Vector3Int newPosition, Peep peep)
    {
        foreach(registeredPeep searchPeep in positions)
        {
            if(searchPeep.peepPos == newPosition)
            {
                return false;
            }
        }

        registeredPeep newPeep = new registeredPeep();
        newPeep.peep = peep;
        newPeep.peepPos = newPosition;
        positions.Add(newPeep);
        return true;
    }

    public void unregisterOldPosition(Vector3Int oldPosition)
    {
        for(int i = 0; i < positions.Count; i++)
        {
            registeredPeep searchPeep = (registeredPeep)positions[i];
            if(searchPeep.peepPos == oldPosition)
            {
                positions.Remove(searchPeep);
                return;
            }
        }
    }

    public Peep GetPeep(Vector3Int position)
    {
        foreach(registeredPeep searchPeep in positions)
        {
            if(searchPeep.peepPos == position)
            {
                return searchPeep.peep;
            }
        }

        Debug.LogWarning("GetPeep called without a Peep Found");
        return null;
    }

    public bool isFreePosition(Vector3Int newPosition)
    {
        foreach (registeredPeep searchPeep in positions)
        {
            if (searchPeep.peepPos == newPosition)
            {
                return false;
            }
        }
        return true;
    }
}
