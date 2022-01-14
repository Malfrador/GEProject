using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepRotatingTileInteract : JobBase
{

    public bool blocked = true; //Not the same as the blocked of peepMovement but essentially. This one is used for waiting until the rotating tile has finished rotating

    private PeepMovement peepMovement;
    public RotateObject rotateObject; //The rotateobject script of the gameobject that represents the rotating tile area. Not actually a tile in a tilemap

    private void Start()
    {
        //Check if we find a second RotatingTileInteract Script other than ourselves. This would mean that we have exited the rotating tile area and can delete ourselfes (and the other script)
        PeepRotatingTileInteract[] allRotatingScripts = gameObject.GetComponents<PeepRotatingTileInteract>();
        if (allRotatingScripts.Length > 1)
        {
            transform.parent = null;
            gameObject.GetComponent<Peep>().removeScripts("PeepRotatingTileInteract");
        }



        //This script gets attached to the peep it has just entered the tile infront of a rotating tile object
        //Thus we can just immediatly block movement and wait for our turn to enter the rotating tile object
        peepMovement = gameObject.GetComponent<PeepMovement>();
        peepMovement.blocked = true;
    }


    public override void doTask(bool oncePerTileCheck)
    {
        //To correctly interface with a rotating tile object we need to 
        // 1. - Block until we get hold of the rotating tile (via raycast)
        // 2. - Check if the rotating tile object is in the process of rotating right now
        // 3  - If it is we block until we know its not rotating anymore
        // 4. - Then we need to check if we're standing infront of a wall of the rotating tile or if its a free corridor
        // 5. - If its not a free corridor check if we're a Miner (by having the miner component) and if yes, then we dig 
        //    - => The actual digging logic is handled by the miner and the rotating tile 
        // 6. - If its free then we can enter the tile and we NEED TO TELL THE ROTATING TILE TO BLOCK. Very important. The rotating tile must not be allowed to rotate with a peep,-
        //    - -thats in the process of moving 
        // 7. - Once it is free, and we blocked the rotating tile then we make the peep become a child of the rotating tile
        // 8. - Then we repeat
        //    - If we're out of the rotating tile a second PeepRotatingTileInteract script is attached to the peep but this script doesnt have to care about that as it gets deleted by the new one (thats what we do in start)
        //    - The other script also makes sure the peep is no longer a child of the rotating tile object

        //Whats not done yet - we need to add a grace period after the rotation of the tile during which peepMovement is still blocked


        //First step. Find the rotating Tile
        if(rotateObject == null)
        {
            //We can just blindly raycast because if the rotating tile is rotating the big box collider is turned off
            //And when its stationary the directional colliders stick out further and get hit first
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f);
            if (hit.collider != null)
            {
                rotateObject = hit.collider.gameObject.GetComponent<RotateObject>();
            }
            else
            {
                //In case we cant find (because its rotating and the collider is somewhere else we dont need to execute all the code after this so we just continue 
                return;
            }
        }

        //Second and third step. As long as we're blocked (its true by default at initialization) check if the rotating tile has finished rotating
        //We're also blocked as soon as the rotatingTile starts rotating
        if(blocked)
        {
            if(!rotateObject.rotiert)
            {
                blocked = false;
            }
            else
            {
                peepMovement.blocked = true;
                return; //If we continue to be blocked theres not point in executing the further code
            }
        }

        //Function moved to rotatingWallDetect and PeepMovement
        //Fourth step and fifth. Wall check and possible mining
        //if(oncePerTileCheck)
        //{
        //    RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right, 1f, LayerMask.GetMask("RotatingTileWalls"));

        //    if (gameObject.GetComponent<Peep>().hasJob(Miner))
        //    {

        //    }
        //    rotateObject.ChangeShape(hit2.collider as BoxCollider2D);
        //}




        //Sixth step. Walking into the tile and telling the tile to stop rotating
        peepMovement.blocked = false;
        if(peepMovement.isMoving())
        {
            rotateObject.blocked = true;
        }
        else
        {
            rotateObject.blocked = false;
        }

        //Seventh step become a child of the rotateObject
        transform.parent = rotateObject.transform;


    }

    public void updateLookDirection(string direction)
    {
        if(direction == "left")
        {
            gameObject.GetComponent<Peep>().lookDirection = peepMovement.relativeDirection("left");
            return;
        }
        else if(direction == "right")
        {
            gameObject.GetComponent<Peep>().lookDirection = peepMovement.relativeDirection("right");
            return;
        }
        Debug.LogError("Unknown look direction update in PeepRotatingTileInteract");
    }

    public bool rotatingTileWallDetected(Vector3 posToCheck)
    {
        RaycastHit2D hit = Physics2D.Raycast(posToCheck, transform.right, 0.25f, LayerMask.GetMask("RotatingTileWalls"));
        if (hit.collider == null)
            return false;

        //If we're a miner we might as well already mine it 
        if(gameObject.GetComponent<Peep>().hasJob(Type.GetType("Miner")))
        {
            rotateObject.ChangeShape(hit.collider as BoxCollider2D);
        }
        else
        {
            return rotateObject.checkWalls(hit.collider as BoxCollider2D);
        }
        return false;
    }
}
