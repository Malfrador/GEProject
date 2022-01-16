using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Miner : JobBase
{
    private Peep peepComponent;
    private TileController tileController;
    private Tilemap tileMap; //Is it better to copy these components at the start or if we just call the tilemap via peepComponent.tilemap everytime we need it?

    private void Start()
    {
        peepComponent = gameObject.GetComponent<Peep>();
        tileController = peepComponent.tileController;
        tileMap = peepComponent.tileMap;

        //Set sprite of Peep to reflect new miner job
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Dev/character_with_pickaxe"); //Sprite of Miner is hard coded here. No bueno D:

        //Revert the miner tile to a normal tile
        tileMap.SetTile(tileMap.WorldToCell(transform.position), tileController.basicBackgroundTile); //TODO: Put the worldtocell functions into peepmovement since it has to do with movement and make a tilecontroller.getbackgroundtile

        //Set Animation Bool to true
        gameObject.GetComponent<Animator>().SetBool("pickaxe",true);
    }

    override public void doTask(bool oncePerTileCheck)
    {
        //We only want the miner job to activate oncePerTile
        if(oncePerTileCheck)
        {

            //TODO: Add code to check for mineable tile and all the mining logic
        }
    }
}
