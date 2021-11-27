using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PeepTileInteraction : JobBase
{

    private Peep peepComponent;
    private TileController tileController;
    private Tilemap tilemap;
    

    private void Start()
    {
        peepComponent = gameObject.GetComponent<Peep>();
        tileController = peepComponent.tileController;
        tilemap = peepComponent.tileMap;
    }

    public override void doTask(bool oncePerTileCheck)
    {
        if(oncePerTileCheck)
        {
            Sprite tempSprite = tilemap.GetSprite(tilemap.WorldToCell(transform.position));
            if (tileController.matchingSprite(tempSprite))
            {
                peepComponent.addScript(tileController.getScriptName(tempSprite));
            }
        }
    }
}
