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
    private char lookDirection = 'E';
    private Vector3 offsetCorrect = new Vector3(0.5f, 0.5f, 0.0f); //Used to correct for the fact that the peep moves on 0.5Coordinates (because hes in the center of tile) while the grid is in whole ints
    public int moveFrequency = 120; //How high moveCounter has to be before the peep makes the next step => How long the peep waits inbetween steps
    public int moveSpeed = 1; //The time it takes for a peep to move from one tile to another
    public Tilemap tileMap;
    public TileWallController wallController;


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
    //This would probably be better in Update() and coupled to deltaTime maybe?
    private void FixedUpdate()
    {

        //We only add to the moveCoutner if the peep is standing still
        if(!moving)
        {
            moveCounter++;
        }


        //This only gets called every 120 times (or however bis moveFrequency is) that FixedUpdate is called
        if(moveCounter >= moveFrequency)
        {
            moveCounter = 0;

            //Desired location stays without offsetCorrect because we want the peep to move to X.5 Y.5 coordinates
            //OffsetCorrect only comes into play when checking the tilemap
            desiredLocation = transform.position + transform.right; 

            //What is the z Coordinate for a TileMap?
            if (!isValidSpot(desiredLocation, tileMap))
                {
                    avoidWall();
                    desiredLocation = transform.position + transform.right;
                }
            moving = true;
        }
    }

    //Decides a new walking direction if the peep hits a wall
    private void avoidWall()
    {
        //First we check if left and right is free. If left and right is free the peep decides at random
        //If left and right is not free the peep walks back
        //If only left or right is free it walks left or right only

        //Check if left or right is valid and then pick a random direction to look in
        //Transform.up is left
        //Trasform.right is forward
        //This is because our character 2D sprite looks to EAST in default orientation
        //Mabye fix this with new Sprites
        bool leftFree = isValidSpot(transform.position + transform.up, tileMap);
        bool rightFree = isValidSpot(transform.position - transform.up, tileMap);
        

        if(leftFree && rightFree)
        {

            //This isnt even the code we want
            //This is for randomly walking left or right but we want individual peeps to walk left, then the next one right, and so on and so forth
            //Im still going to leave this here tough

            //Does this produce right rounding?
            //switch (Mathf.RoundToInt(Random.Range(100.0f, 200.0f) / 100.0f))
            //{
            //    case 1:
            //        changeDirection(relativeDirection("right"));
            //        Debug.Log("Random right");
            //        break;
            //    case 2:
            //        changeDirection(relativeDirection("left"));
            //        Debug.Log("Random left");
            //        break;
            //}

            //Instead we track the number of times a peep has hit a wall with both sides (left&right) being empty on a WallTileController Object that has 2D Array of ints
            //Whereing the first dimension is the x and the second the y coordinate of the tile position
            //But we never actually interface with the tileMap here
            if (wallController != null)
            {
                //Now we check how many uses the wallController says the tile at our desired location had and move left or right accordingly. And we ofc increment the hits
                if (wallController.getNumberOfHits(Mathf.RoundToInt(desiredLocation.x + offsetCorrect.x), Mathf.RoundToInt(desiredLocation.y + offsetCorrect.y)) % 2 == 0)
                {
                    changeDirection(relativeDirection("left"));
                }
                else
                {
                    changeDirection(relativeDirection("right"));
                }
                wallController.addNumberOfHits(Mathf.RoundToInt(desiredLocation.x + offsetCorrect.x), Mathf.RoundToInt(desiredLocation.y + offsetCorrect.y), 1);

            }
            else
            {
                Debug.LogError("TileWallController missing");
            }
                
            
            return;
        }
        if(leftFree)
        {
            changeDirection(relativeDirection("left"));
            return;
        }
        if(rightFree)
        {
            changeDirection(relativeDirection("right"));
            return;
        }

        //If we've come this far we can be sure that were in a dead end and the only valid position is backwards 
        changeDirection(relativeDirection("back"));

    }


    //TODO: Look directions mit Enum machen damit man einfach per int und char auf direction zugreifen kann
    private char relativeDirection(string newDirection)
    {
        switch(lookDirection)
        {

            case 'E':
                switch(newDirection)
                {
                    case "right":
                        return 'S';
                    case "left":
                        return 'N';
                    case "back":
                        return 'W';
                }
                break;


            case 'N':
                switch (newDirection)
                {
                    case "right":
                        return 'E';
                    case "left":
                        return 'W';
                    case "back":
                        return 'S';
                }
                break;


            case 'W':
                switch (newDirection)
                {
                    case "right":
                        return 'N';
                    case "left":
                        return 'S';
                    case "back":
                        return 'E';
                }
                break;


            case 'S':
                switch (newDirection)
                {
                    case "right":
                        return 'W';
                    case "left":
                        return 'E';
                    case "back":
                        return 'N';
                }
                break;
        }

        return 'E';
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

    

    //Passing the tilemap isnt even necessary if were in the same class
    //Would be tough if it would be some kind of Util-Class
    private bool isValidSpot(Vector3 positionToCheck, Tilemap tilemapForSprite)
    {
        
        if(tilemapForSprite.GetTile(tilemapForSprite.WorldToCell(positionToCheck)).GetType().ToString() == "WallTile")
        {
            return false;
        }

        return true;
    }

    //This isnt even used anymore anywhere. Im gonna leave it here tough for now
    //private Vector3Int Vector3ToInt(Vector3 vector3)
    //{
    //    return new Vector3Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
    //}

    //Not even sure why this is still here. It cant hurt to keep it tough i guess
    //private void move()
    //{

    //    //First we set our accurate position according to where were looking
    //    accuratePosition = accuratePosition + new Vector3Int(Mathf.RoundToInt(transform.right.x), Mathf.RoundToInt(transform.right.y), Mathf.RoundToInt(transform.right.z));

    //    //And now move according to oure accuratePosition
    //    //Hopefully Vector3Int to Vector3 works lol
    //    transform.position = accuratePosition;
    //}

}
