using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PeepMovement : JobBase
{
    //FixedUpdate gets called 30 or so times every second but we only want the Character to move every few Seconds
    //Either we do this by checking deltaTime or by using a counter
    //Maybe theres a better way but im going with a counter for now
    private int moveCounter = 0;

    private Vector3 desiredLocation = new Vector3(0, 0, 0); //Here we save where the Peep is supposed to move to during the visual moving that gets performed in Update()
    private Vector3 offsetCorrect = new Vector3(0.5f, 0.5f, 0.0f); //Used to correct for the fact that the peep moves on 0.5Coordinates (because hes in the center of tile) while the grid is in whole ints
    private bool moving = false; //Gets changed when the Peep is actually moving visually
    private bool oncePerTileCheckBuffer = false; //Needed because we can only know if the oncePerTileCheck is ok in our update function since thats where we move. But if the oncePerTile in the update theres possible desync with the FixedUpdate where all the job logic gets handled, so it gets buffered for the next call of doTask()
    public int moveFrequency = 40; //How high moveCounter has to be before the peep makes the next step => How long the peep waits inbetween steps
    public int moveSpeed = 1; //The time it takes for a peep to move from one tile to another
    public bool blocked = false; //This is just for outside use. Nothing inside PeepMovement should change this. This is so that other jobs on the peep can block the movement

    public Tilemap tileMap;
    public TileController tileController;
    private PeepController peepController;
    private Peep mainPeepComponent;

    private void Start()
    {
        //"copy" over the tilemap and tilewallcontroller from the main peep component 
        //Store mainPeepComponent for later uses to get look direction
        //Look direction is stored in peep component => copying could lead to async
        mainPeepComponent = gameObject.GetComponent<Peep>();
        tileMap = mainPeepComponent.tileMap;
        tileController = mainPeepComponent.tileController;
        peepController = mainPeepComponent.peepController;
    }

    private void Update()
    {

        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredLocation, moveSpeed * Time.deltaTime);
        }

        //Can the == in Vector3 handle float inaccuracies?
        //This activates every frame once we arrive at our desired location
        if (transform.position == desiredLocation)
        {
            //This *should* only activate once when we first arrive at our desired location
            if (!mainPeepComponent.oncePerTileCheck && moving)
            {
                oncePerTileCheckBuffer = true;
                peepController.unregisterOldPosition(CustomUtil.Vector3ToInt(transform.position - transform.right + offsetCorrect));
            }
            moving = false;
        }

        Debug.DrawRay(transform.position + transform.right, transform.right, Color.green);
    }

    override public void doTask(bool oncePerTileCheck) //Once per tile check isnt needed here but we still *need* to accept it nontheless. I think...
    {

        //This ensures that the oncePerTile check gets done exactly once for every job. We cant be 100% sure if the PeepMovement is the first job that gets its doTask() executed
        //So we buffer the oncePerTileCheck and the set it in the doTask() which actually gets called by FixedUpdate in the main peep script
        //So that the oncePerTile check runs for every job only once, with all Jobs in a list the oncePerTileCheck bool of the main peep script would look like this
        //                  Run 1       Run 2
        //  Task X          false       true
        //  Task Y          false       true
        //  PeepMovement    true        false
        //  Task Z          true        false
        //  Task W          true        false
        if (oncePerTileCheck)
        {
            mainPeepComponent.oncePerTileCheck = false;
        }
        if(oncePerTileCheckBuffer)
        {
            oncePerTileCheckBuffer = false;
            mainPeepComponent.oncePerTileCheck = true;
        }
            
        //The oncePerTileCheck stuff still needs to be done even if the peep is blocked because a job in the first "check cycle" could block the movement and thus
        //the other half of jobs maybe wouldnt get to execute their doTask()
        //We also only have to check for blocked in toTask since if "moving" is true all the moving stuff gets done in update() 
        //And thus other jobs just have to check if the peep is moving before or after blocking it
        if (blocked)
        {
            return;
        }    



        //We only add to the moveCoutner if the peep is standing still
        if (!moving)
        {
            moveCounter++;
        }

        //This only gets called every 120 times (or however bis moveFrequency is) that FixedUpdate is called
        if (moveCounter >= moveFrequency)
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


            //After having checked all the walls for free space we need to check if theres a peep already in the space we're trying to walk into
            //For that we raycast and see if we hit the 2d collider of the other peep
            //If so we just skip this "moving Cycle" and check again in however long moveFrequency takes.
            if(isForwardFree())
            {
                moving = true;
            }
            else
            {
                //Check if the peep thats infront of us looks in the same direction
                //If it doesnt then we can simply turn around
                Peep otherPeep = peepController.GetPeep(CustomUtil.Vector3ToInt(transform.position + transform.right));
                if(otherPeep != null) //NO Idea why peeps sometimes thing that Forward isnt free and check in empty spots but i dont have time right now to test
                {
                    if (otherPeep.lookDirection == mainPeepComponent.lookDirection)
                    {
                        changeDirection(relativeDirection("back"));
                    }
                }
            }
        }
    }
    
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


        if (leftFree && rightFree)
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
            if (tileController != null)
            {
                //Now we check how many uses the wallController says the tile at our desired location had and move left or right accordingly. And we ofc increment the hits
                if (tileController.getNumberOfHits(Mathf.RoundToInt(desiredLocation.x + offsetCorrect.x), Mathf.RoundToInt(desiredLocation.y + offsetCorrect.y)) % 2 == 0)
                {
                    changeDirection(relativeDirection("left"));
                }
                else
                {
                    changeDirection(relativeDirection("right"));
                }
                tileController.addNumberOfHits(Mathf.RoundToInt(desiredLocation.x + offsetCorrect.x), Mathf.RoundToInt(desiredLocation.y + offsetCorrect.y), 1);

            }
            else
            {
                Debug.LogError("TileWallController missing");
            }


            return;
        }
        if (leftFree)
        {
            changeDirection(relativeDirection("left"));
            return;
        }
        if (rightFree)
        {
            changeDirection(relativeDirection("right"));
            return;
        }

        //If we've come this far we can be sure that were in a dead end and the only valid position is backwards 
        changeDirection(relativeDirection("back"));

    }

    public bool isForwardFree()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right, transform.right, 0.5f); //Magic number for length of Raycast. Also we need to move the raycast origin or else we hit our own collider D:
        //if(hit.collider != null)
        //    return hit.collider.gameObject.CompareTag("Peep");
        //return false;
        //We only have to register our new postion at the peepController. There should be no raycasts needed
        return peepController.registerPosition(CustomUtil.Vector3ToInt(transform.position + transform.right + offsetCorrect), mainPeepComponent);
    }

    //TODO: Look directions mit Enum machen damit man einfach per int und char auf direction zugreifen kann
    public char relativeDirection(string newDirection)
    {
        switch (mainPeepComponent.lookDirection)
        {

            case 'E':
                switch (newDirection)
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
        mainPeepComponent.lookDirection = newDirection;

        //very nice that switch works with char in C#
        //Not sure if just using eulerAngles and "force setting" all the other rotations to 0 is a good idea
        //Mabye we'll need the rotations for something else? Idk
        switch (newDirection)
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
        if (tilemapForSprite.GetTile(tilemapForSprite.WorldToCell(positionToCheck)).GetType().ToString() == "WallTile")
        {
            return false;
        }
        if(mainPeepComponent.hasJob(Type.GetType("PeepRotatingTileInteract"))) //If we detect that we have a PeepRotatingTileInteract that means we also have to check the walls of the rotating tile
        {
            PeepRotatingTileInteract peepRotatingTileInteract = mainPeepComponent.getJob(Type.GetType("PeepRotatingTileInteract")) as PeepRotatingTileInteract;
            peepRotatingTileInteract.rotatingTileWallDetected(positionToCheck);
        }

        return true;
    }

    public bool isMoving()
    {
        return moving;
    }


    

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
