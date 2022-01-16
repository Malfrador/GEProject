using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameCoordinator : MonoBehaviour
{

    public UnityEvent pauseEvent = new UnityEvent(); //This is the event that broadcasts to all "GamePauseListeners" that the game is paused
    public UnityEvent trapMarkersClear = new UnityEvent(); //The event that triggers to clear all markers

    public Camera mainCamera;
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public Text peepsLeftText;
    public GameObject blendCanvas;
    public GameObject protoPeep;
    public GameObject trapMarker;
    public GameObject raft;
    public float timeBetweenPeepSpawns = 2; //Time between spawns in seconds
    public int winningPeeps = 5;

    private Transform spawnPoint;
    private Tilemap tileMap;
    private TileController tileController;
    private PeepController peepController;
    private AudioController audioController;

    private float timeBetweenLastSpawn = 9999; //Is setto high number so a peep instantly spawns when level starts
    private bool gameRunning = true;
    private Vector3 offsetCorrect = new Vector3(0.5f, 0.5f, 0.0f); //Used to correct for the fact that the spawnPoints are offset from grid (since the spawn needs to be in center of grid)

    private int numberOfPeepsSpawned = 0; //DEBUG REMOVE
    private void Start()
    {
        DontDestroyOnLoad(menuCanvas); //Has to be done here because both canvases are not enabled when starting the game
        DontDestroyOnLoad(gameCanvas);
        audioController = gameObject.GetComponent<AudioController>();
        

        //DEBUG REMOVE 
        OnLevelWasLoaded();
        //DEBUG END
    }

    private void Update()
    {

        //DEBUG THIS IS FOR LOADING THE NEXT SCENE
        if (Input.GetKeyDown(KeyCode.L))
        {
            levelWon();
        }
        //DEBUG END



        //During our level runing we have to 
        // - Spawn peeps at specific intervals
        // - Only do all of this if the game is not paused
        if (gameRunning)
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

    public void activateRaft()
    {
        Vector2[] rafts = tileController.raftPositions;
        foreach(Vector2 newRaftPos in rafts)
        {
            Instantiate(raft, newRaftPos, Quaternion.identity);
        }
    }

    public void causeTrapClearing(Vector2[] trapLocations)
    {
        //This is for when a vulcano causes a trap clearing
        //To clear a single trap we need to:
        // - Pause the game
        // - Put translucent sprites over all traps with hitboxes
        // - Give the trap markers a reference to ourself so they can activate clearTrap 
        // - => New Method then does the actual clearing
        // - => This method only initializes the "trap clearing sprites" i.e the little green boxes over the traps that can be cleared

        //Pause the game
        //TODO: Disable pause button otherwise we could re-run time
        gameRunning = false;
        pauseEvent.Invoke();

        //Initialize all markers
        foreach(Vector2 newTrapLoc in trapLocations)
        {
            GameObject newMarker = Instantiate(trapMarker, newTrapLoc, Quaternion.identity);
            TrapMarker newTrapMarkerComp = newMarker.GetComponent<TrapMarker>();
            newTrapMarkerComp.gameCoordinator = this;
        }
    }

    public void clearTrap(Vector2 trapLocation)
    {
        trapMarkersClear.Invoke();
        gameRunning = true;

        //Clear the tile
        tileMap.SetTile(tileMap.WorldToCell(trapLocation), tileController.basicBackgroundTile);
    }

    public void OnLevelWasLoaded()
    {
        //At the start of a level we have to 
        // - Disable or enable the UIs (Game or MainMenu UI)
        // - Check if we're in a playable Scene - i.e not the MainMenu
        // - Set the camera accordingly - if we are in a playable scene search for the "ReferenceCamera" and copy position and size from that to our camera
        // - If we're not in a playable scene set camera position to 0,0 and size to 5
        // - Find the spawnpoint where peeps are supposed to spawn from
        // - Remove the debug sprite from the spawnpoint thats used for easy leveldesign
        // - Find the TileMap
        // - Find the TileController of the level, the tileController is always different because it houses the sprites that match the specific tileset in use, maybe automate this?
        // - Find hte PeepController of the level
        // - Set the amount of peeps needed to win this level

        blendCanvas.GetComponent<Animator>().Play("BlendOutAnimation");
        gameCanvas.enabled = gameObject.GetComponent<SceneController>().playableScene();
        menuCanvas.gameObject.SetActive(false); //Menu Canvas always and only gets enabled by the animator of the main menu background



        if (!gameObject.GetComponent<SceneController>().playableScene())
        {
            gameRunning = false;
            mainCamera.orthographicSize = 5;
            mainCamera.transform.position = new Vector3(0, 0, -10);
            return;
        }
        else
        {
            //Find ReferenceCamera and set position and size accordingly
            Camera referenceCam = null;
            Camera[] cameras = FindObjectsOfType<Camera>();
            foreach(Camera cam in cameras)
            {
                if(cam.gameObject.name == "ReferenceCamera")
                {
                    referenceCam = cam;
                }
            }
            if(referenceCam != null)
            {
                mainCamera.transform.position = new Vector3(referenceCam.transform.position.x, referenceCam.transform.position.y, -10);
                mainCamera.orthographicSize = referenceCam.orthographicSize;
                referenceCam.gameObject.SetActive(false);
            }
        }
        gameRunning = true;
        GameObject spawnPointObject = GameObject.Find("Spawnpoint");
        spawnPoint = spawnPointObject.transform;
        if (spawnPointObject.GetComponent<SpriteRenderer>())
            spawnPointObject.GetComponent<SpriteRenderer>().enabled = false;
        tileMap = GameObject.FindObjectOfType<Tilemap>();
        tileController = GameObject.FindObjectOfType<TileController>();
        peepController = GameObject.FindObjectOfType<PeepController>();
        winningPeeps = 5;
        peepsLeftText.text = tileController.peepsSpawnable.ToString();
    }

    private void spawnPeep()
    {
        //To spawn a peep we have to
        // - Check in the TileController (because its unique in every scene) how many peeps are left
        // - Check if the spawnLocation is free
        // - Initialize the Prefab
        // - Set the Tilemap for the new Peep
        // - Set the tilemapcontroller for the new peep
        // - Set the gamecoordinator for the peep to this one
        // - Set the peepcontroller for the new peep
        // - Set the correct rotation, the peep looks to the right with 0 'z' rotation, add +90 onto the rotation of our spawnPoint transform
        // - Set the right position
        // - Play the sound

        
        if(tileController.peepsSpawnable > 0)
        {
            tileController.peepsSpawnable--;
            peepsLeftText.text = tileController.peepsSpawnable.ToString();
        }
        else
        {
            return;
        }

        if (!peepController.isFreePosition(CustomUtil.Vector3ToInt(spawnPoint.position + offsetCorrect)))
        {
            Debug.LogWarning("Game Controller tried to spawn in occupied position");
            return;
        }
        GameObject newPeep = Instantiate(protoPeep);
        peepController.registerPosition(CustomUtil.Vector3ToInt(spawnPoint.position + offsetCorrect), newPeep.GetComponent<Peep>());
        newPeep.GetComponent<Peep>().tileMap = tileMap;
        newPeep.GetComponent<Peep>().tileController = tileController;
        newPeep.GetComponent<Peep>().gameCoordinator = this;
        newPeep.GetComponent<Peep>().peepController = peepController;
        newPeep.GetComponent<Peep>().lookDirection = CustomUtil.angleToLookDir(Mathf.RoundToInt(spawnPoint.rotation.eulerAngles.z) + 90);
        newPeep.transform.rotation = Quaternion.Euler(0, 0, spawnPoint.rotation.eulerAngles.z + 90.0f);
        newPeep.transform.position = spawnPoint.position;
        audioController.playClip("peepSpawn");

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

        audioController.playClip("peepFinish");
    }

    private void levelWon()
    {
        //For now this just loads the next level
        //In the future this should obviously pause the game and call something in the ui
        //And then the ui calls the level loading 
        audioController.playClip("gameWon");
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
