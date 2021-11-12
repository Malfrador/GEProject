using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Peep : MonoBehaviour
{
    //FixedUpdate gets called 30 or so times every second but we only want the Character to move every few Seconds
    //Either we do this by checking deltaTime or by using a counter
    //Maybe theres a better way but im going with a counter for now
    private int moveCounter = 0;

    //Not sure if this is needed. It seems to work fine just with float but i'll leave it hear in case in the future it is needed
    //
    //We need to store our positions as solid int's because if we add the positon offset when moving to our current position
    //then the float inaccuracies will add up over time leading to use wandering off grid
    //Then we just need to add the float offset (-0.5 on x and -0.5 on y to be aligned with the tilemap again => because we want to be in the center of tiles)
    //Should these be public?
    //private Vector3Int accuratePosition = new Vector3Int(0, 0, 0);


    private Vector3 desiredLocation = new Vector3(0, 0, 0); //Here we save where the Peep is supposed to move to during the visual moving that gets performed in Update()
    private bool moving = false; //Gets changed when the Peep is actually moving visually
    private char lookDirection = 'N';
    public int moveFrequency = 120;
    public int moveSpeed = 1; //The time it takes for a peep to move from one tile to another
    public Tilemap tileMap;

    public Sprite wallSprite; //This is the sprite that should be avoided TODO:Change this so we can maybe avoid walls based on the Tag they have

    private void Update()
    {
        
        if(moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredLocation, moveSpeed * Time.deltaTime);
        }

        //Can the == in Vector3 handle float inaccuracies?
        if (transform.position == desiredLocation)
        {
            moving = false;
        }
    }

    //Deciding when to move is executed in FixedUpdate because we dont want peeps moving faster or slower depending on framerate
    //The actual visual moving is done in Upadate for frame rate smoothness
    private void FixedUpdate()
    {

        //We only add to the moveCoutner if the peep is standing still
        if(!moving)
        {
            moveCounter++;
        }


        //This only gets called every 120 times that FixedUpdate is called
        if(moveCounter >= moveFrequency)
        {
            moveCounter = 0;

            //Just for testing purpose
            //===========================
            switch((int)Random.Range(1, 4))
            {
                case 1:
                    lookDirection = 'N';
                    break;
                case 2:
                    lookDirection = 'S';
                    break;
                case 3:
                    lookDirection = 'W';
                    break;
                case 4:
                    lookDirection = 'E';
                    break;
            }

            changeDirection(lookDirection);
            //============================

            //Moving ABSOLUTELY needs to be done AFTER changing where the peep is looking otherwise we'll get the wrong desired position
            desiredLocation = transform.position + transform.right;

            //What is the z Coordinate for a TileMap?
            if (isValidSpot(wallSprite, new Vector3Int(Mathf.RoundToInt(desiredLocation.x), Mathf.RoundToInt(desiredLocation.y), 0), tileMap))
            {
                moving = true;
            }
        }
    }

    private void changeDirection(char newDirection)
    {
        //Just for safety
        lookDirection = newDirection;

        //very nice that switch works with char in C#
        //Not sure if just using eulerAngles and "force setting" all the other rotations to 0 is a good idea
        //Mabye we'll need the rotations for something else? Idk
        switch(newDirection)
        {
            case 'E':
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 'N':
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 'W':
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 'S':
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }
    }

    //private void move()
    //{

    //    //First we set our accurate position according to where were looking
    //    accuratePosition = accuratePosition + new Vector3Int(Mathf.RoundToInt(transform.right.x), Mathf.RoundToInt(transform.right.y), Mathf.RoundToInt(transform.right.z));

    //    //And now move according to oure accuratePosition
    //    //Hopefully Vector3Int to Vector3 works lol
    //    transform.position = accuratePosition;
    //}

    private bool isValidSpot(Sprite wallSpriteToCheck, Vector3Int positionToCheck, Tilemap tilemapForSprite)
    {

        if(tilemapForSprite.GetSprite(positionToCheck) != wallSpriteToCheck)
        {
            return true;
        }

        return false;
    }
}
