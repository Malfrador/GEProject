using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    //This is used to track how often a peep hits a certain tile
    //The top list is the 'x' component of the tile position
    //The second dimension is the 'y' position
    //The actual content is the number of hits the tile had
    //So you might have an Array with an entry at '2' and then the next one at '5' with 3 free spaces inbetween
    //But at the moment i dont know another way to centrally control the peep tile hits
    private int[,] hitData = new int[100, 100]; //Magic Number yey

    public Tile basicBackgroundTile; //This is for scripts to replace a tile if the powerup (for example the miner powerup) is exhausted and we want to replace the pickaxe tile with a normal background
    public Sprite doorTileSprite;

    public Sprite pickaxeTileSprite; //We use a bunch of public Sprites to set what sprite does what logic function. Then the individual jobs from peep can just access theses sprites
    public string pickaxeScript; //And the script associated with the pickaxeTile

    public Sprite goalTileSprite; //The sprite of the goal tile
    public string goalScript; //The script that counts the peep as having won, deletes the peep and adds to the "winning peep counter"

    public Sprite rotatingTileSprite; //The sprite thats below the rotating tile object
    public string rotatingTileScript; //The script that manages interaction with the rotating tile object

    public Sprite keyTileSprite; //The sprite of the Key pickup. Not the sprite of the Key-Door Tile
    public string keyTileScript; //The script that manages interaction with the key, picks it up and unlocks the door


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

    //This checks trough all Sprites to see if we know if the Sprite getting checked has any known assocations with a script
    public bool matchingSprite(Sprite spriteToCompare)
    {
        //Ok. SURELY there has to be a better way
        if (spriteToCompare == goalTileSprite)
            return true;
        if (spriteToCompare == pickaxeTileSprite)
            return true;
        if (spriteToCompare == rotatingTileSprite)
            return true;
        if (spriteToCompare == keyTileSprite)
            return true;

        return false;
    }

    //This gets the script thats associated with a Sprite
    public string getScriptName(Sprite associatedSprite)
    {
        //Is there *ANY* better way to do this? 
        //Iterating trough Array has the same performance but is far less user friendly in the editor to assign new Script-Sprite combos because both are Arrays
        //At the very least make it so that the least used association is the last in the if list
        //Switch is not possible because it expects constants 
        if (associatedSprite == goalTileSprite)
            return goalScript;
        if (associatedSprite == pickaxeTileSprite)
            return pickaxeScript;
        if (associatedSprite == rotatingTileSprite)
            return rotatingTileScript;
        if (associatedSprite == keyTileSprite)
            return keyTileScript;

        Debug.LogError("getScriptName() called without matching sprite");
        return null;
    }
}
