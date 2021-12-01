using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameCoordinator : MonoBehaviour
{

    public GameObject protoPeep;
    public float timeBetweenPeepSpawns = 2; //Time between spawns in seconds
    public int winningPeeps = 5;

    private Transform spawnPoint;
    private Tilemap tileMap;
    private TileController tileController;

    private float timeBetweenLastSpawn = 0;

    private void Start()
    {
        //DEBUG REMOVE 
        OnLevelWasLoaded();
        //DEBUG END
    }

    private void Update()
    {
        timeBetweenLastSpawn += Time.deltaTime;
        if(timeBetweenLastSpawn >= timeBetweenPeepSpawns)
        {
            //Add a check if theres already a peep on the tile
            spawnPeep();
            timeBetweenLastSpawn = 0;
        }
    }

    private void FixedUpdate()
    {
        //During our level runing we have to 
        // - Spawn peeps at specific intervals
        
    }

    public void OnLevelWasLoaded()
    {
        //At the start of a level we have to 
        // - Find the spawnpoint where peeps are supposed to spawn from
        // - Remove the debug sprite from the spawnpoint thats used for easy leveldesign
        // - Find the TileMap
        // - Find the TileController of the level, the tileController is always different because it houses the sprites that match the specific tileset in use, maybe automate this?
        // - Set the amount of peeps needed to win this level

        GameObject spawnPointObject = GameObject.Find("Spawnpoint");
        spawnPoint = spawnPointObject.transform;
        if(spawnPointObject.GetComponent<SpriteRenderer>())
            spawnPointObject.GetComponent<SpriteRenderer>().enabled = false;
        tileMap = GameObject.FindObjectOfType<Tilemap>();
        tileController = GameObject.FindObjectOfType<TileController>();
        winningPeeps = 5;
    }

    private void spawnPeep()
    {
        //To spawn a peep we have to
        // - Initialize the Prefab
        // - Set the Tilemap for the new Peep
        // - Set the tilemapcontroller for the new peep
        // - Set the gamecoordinator for the peep to this one
        // - Set the correct rotation, the peep looks to the right with 0 'z' rotation, add +90 onto the rotation of our spawnPoint transform
        // - Set the right position

        //I think this is not necessary
        // - Activate the new Peep (the components of the peep expect the tilemap and tilecontroller to be assigned at Start() so the prefab is deactivated

        GameObject newPeep = Instantiate(protoPeep);
        newPeep.GetComponent<Peep>().tileMap = tileMap;
        newPeep.GetComponent<Peep>().tileController = tileController;
        newPeep.GetComponent<Peep>().gameCoordinator = this;
        newPeep.transform.rotation = Quaternion.Euler(0, 0, spawnPoint.rotation.eulerAngles.z + 90.0f);
        newPeep.transform.position = spawnPoint.position;
    }

    public void peepGoalReached()
    {
        winningPeeps--;
        if(winningPeeps <= 0)
        {
            Debug.Log("Level won");
        }
    }
}
