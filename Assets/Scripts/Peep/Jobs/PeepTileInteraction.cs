using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PeepTileInteraction : JobBase
{

    private Peep peepComponent;
    private TileController tileController;
    private Tilemap tilemap;
    private Vector3 offsetCorrect = new Vector3(0.5f, 0.5f, 0.0f);

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
            Debug.Log(tilemap.GetTile(tilemap.WorldToCell(transform.position)).GetType().ToString());
            if(tilemap.GetTile(tilemap.WorldToCell(transform.position)).GetType().ToString() == "WaterTile")
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
                if(hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Raft")
                    {
                        return;
                    }
                }
                peepComponent.die();

            }
        }
    }
}
