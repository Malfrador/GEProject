using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class GameCoordinator : MonoBehaviour
{

    public UnityEvent pauseEvent = new UnityEvent(); //This is the event that broadcasts to all "GamePauseListeners" that the game is paused

    public GameObject protoPeep;
    public float timeBetweenPeepSpawns = 2; //Time between spawns in seconds
    public int winningPeeps = 5;

    private Transform spawnPoint;
    private Tilemap tileMap;
    private TileController tileController;
    private PeepController peepController;

    private float timeBetweenLastSpawn = 9999; //Is setto high number so a peep instantly spawns when level starts
    private bool gameRunning = true;
    private Vector3 offsetCorrect = new Vector3(0.5f, 0.5f, 0.0f); //Used to correct for the fact that the spawnPoints are offset from grid (since the spawn needs to be in center of grid)

    private int numberOfPeepsSpawned = 0; //DEBUG REMOVE
    private void Start()
    {


        //DEBUG REMOVE 
        OnLevelWasLoaded();
        //DEBUG END
    }

    private void Update()
    {

        //DEBUG THIS IS FOR LOADING THE NEXT SCENE
        if(Input.GetKeyDown(KeyCode.L))
        {
            levelWon();
        }
        //DEBUG END



        //During our level runing we have to 
        // - Spawn peeps at specific intervals
        // - Only do all of this if the game is not paused
        if(gameRunning)
        {
            timeBetweenLastSpawn += Time.deltaTime;
            if (timeBetweenLastSpawn >= timeBetweenPeepSpawns)
            {
                //Add a check if theres already a peep on the tile
                spawnPeep();
                timeBetweenLastSpawn = 0;
            }
        }
        


        

    }

    private void FixedUpdate()
    {
        
        
    }

    public void OnLevelWasLoaded()
    {
        //At the start of a level we have to 
        // - Find the spawnpoint where peeps are supposed to spawn from
        // - Remove the debug sprite from the spawnpoint thats used for easy leveldesign
        // - Find the TileMap
        // - Find the TileController of the level, the tileController is always different because it houses the sprites that match the specific tileset in use, maybe automate this?
        // - Find hte PeepController of the level
        // - Set the amount of peeps needed to win this level

        GameObject spawnPointObject = GameObject.Find("Spawnpoint");
        spawnPoint = spawnPointObject.transform;
        if(spawnPointObject.GetComponent<SpriteRenderer>())
            spawnPointObject.GetComponent<SpriteRenderer>().enabled = false;
        tileMap = GameObject.FindObjectOfType<Tilemap>();
        tileController = GameObject.FindObjectOfType<TileController>();
        peepController = GameObject.FindObjectOfType<PeepController>();
        winningPeeps = 5;
    }

    private void spawnPeep()
    {
        //To spawn a peep we have to
        // - Check if the spawnLocation is free
        // - Initialize the Prefab
        // - Set the Tilemap for the new Peep
        // - Set the tilemapcontroller for the new peep
        // - Set the gamecoordinator for the peep to this one
        // - Set the peepcontroller for the new peep
        // - Set the correct rotation, the peep looks to the right with 0 'z' rotation, add +90 onto the rotation of our spawnPoint transform
        // - Set the right position

        //I think this is not necessary
        // - Activate the new Peep (the components of the peep expect the tilemap and tilecontroller to be assigned at Start() so the prefab is deactivated
        if (!peepController.registerPosition(CustomUtil.Vector3ToInt(transform.position + offsetCorrect)))
        {
            Debug.LogWarning("Game Controller tried to spawn in occupied position");
            return;
        }
        GameObject newPeep = Instantiate(protoPeep);
        newPeep.GetComponent<Peep>().tileMap = tileMap;
        newPeep.GetComponent<Peep>().tileController = tileController;
        newPeep.GetComponent<Peep>().gameCoordinator = this;
        newPeep.GetComponent<Peep>().peepController = peepController;
        newPeep.transform.rotation = Quaternion.Euler(0, 0, spawnPoint.rotation.eulerAngles.z + 90.0f);
        newPeep.transform.position = spawnPoint.position;

        newPeep.name = newPeep.name + numberOfPeepsSpawned.ToString(); //DEBUG REMOVE
        numberOfPeepsSpawned++;
    }

    public void peepGoalReached()
    {
        winningPeeps--;
        if(winningPeeps <= 0)
        {
            Debug.Log("Level won");
        }
    }

    private void levelWon()
    {
        //For now this just loads the next level
        //In the future this should obviously pause the game and call something in the ui
        //And then the ui calls the level loading 

        gameObject.GetComponent<SceneController>().loadNextScene();

    }

    public void pauseButtonClick()
    {
        pauseEvent.Invoke();
        gameRunning = !gameRunning;
    }

    public void ffButtonClick()
    {
        Time.timeScale = Time.timeScale == 20 ? 1 : 20;
    }


}
