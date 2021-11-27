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
