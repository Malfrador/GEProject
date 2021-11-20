using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWallController : MonoBehaviour
{
    //This is used to track how often a peep hits a certain tile
    //The top list is the 'x' component of the tile position
    //The second dimension is the 'y' position
    //The actual content is the number of hits the tile had
    //So you might have an Array with an entry at '2' and then the next one at '5' with 3 free spaces inbetween
    //But at the moment i dont know another way to centrally control the peep tile hits
    private int[,] hitData = new int[100, 100]; //Magic Number yey

    public int getNumberOfHits(int posX, int posY)
    {
        //We have to modify the posX and posY coordinates because as input we get negative coordinates but we only want positive ones in the array
        //So the world coordinate '0' is the ArrayPosition '50' in our array
        return hitData[posX + (hitData.GetLength(0) / 2), posY + (hitData.GetLength(1) / 2)]; //Can this ever be null? IDKKK
    }

    public void addNumberOfHits(int posX, int posY, int numberOfHits)
    {
        hitData[posX + (hitData.GetLength(0) / 2), posY + (hitData.GetLength(1) / 2)] += numberOfHits;
    }
}
