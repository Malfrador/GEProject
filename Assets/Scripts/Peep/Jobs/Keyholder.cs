using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Keyholder : JobBase
{
    //Alot of this code is copied straight from Miner.cs


    private Peep peepComponent;
    private TileController tileController;
    private Tilemap tileMap;

    private void Start()
    {
        peepComponent = gameObject.GetComponent<Peep>();
        tileController = peepComponent.tileController;
        tileMap = peepComponent.tileMap;

        //Set sprite of Peep to reflect new Keyholder
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Resources/Textures/Dev/character_with_key");


        // Revert the Key tile to a normal tile
        tileMap.SetTile(tileMap.WorldToCell(transform.position), tileController.basicBackgroundTile);

        //Set Animation Bool to true
        gameObject.GetComponent<Animator>().SetBool("key", true);
    }

    override public void doTask(bool oncePerTileCheck)
    {

        //Unlocking door logic
        if (oncePerTileCheck)
        {
            //To unlock the door we have to
            // - Check every tile if the Tile infront of us has the same sprite as a door
            // - Change the tile infront of us to a normal background tile 
            // - Revert back to a peep without the key
            if(tileMap.GetSprite(tileMap.WorldToCell(transform.position + transform.right)) == tileController.doorTileSprite)
            {
                //Set Animation Bool to true
                gameObject.GetComponent<Animator>().SetBool("use_key", true);

                //Set the tile infront of use back to normal tile
                tileMap.SetTile(tileMap.WorldToCell(transform.position + transform.right), tileController.basicBackgroundTile);


                //Revert back to being a normal peep
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Resources/Textures/Dev/character"); //Do this back to peep?
                peepComponent.removeScripts("Keyholder");
            }
        }
    }
}
