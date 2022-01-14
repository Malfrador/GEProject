using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Peep : MonoBehaviour
{
   
    private ArrayList jobs = new ArrayList();


    public char lookDirection = 'E';
    public bool oncePerTileCheck = false; //This is true a single time when a peep moves into a new tile and gets passed onto every doTask() of every job, gets set by PeepMovemet
    public Tilemap tileMap;
    public TileController tileController;
    public GameCoordinator gameCoordinator;
    public PeepController peepController;

    private void Start()
    {
        //Would initalizing PeepMovement from this script be better instead of relying on it being on the peep?
        jobs.Add(gameObject.GetComponent<PeepMovement>());
        jobs.Add(gameObject.GetComponent<PeepTileInteraction>());
    }

    //For now the entire code in PeepMovement relies on getting called in every FixedUpdate. This *may* be nice for pausing the game 
    //But im unsure at the moment if maybe doing it via deltaTime would be better
    private void FixedUpdate()
    {
        runTasks();
    }

    //Goes trough all jobs the component knows of and activates their doTask() method
    private void runTasks()
    {
        //Have to do this instead of using foreach because foreach gets confused if we add something to the jobs Arraylist while iterating trough it (if a job adds another jobs)
        for(int i = 0; i < jobs.Count; i++)
        {
            JobBase jobInstance = jobs[i] as JobBase;
            if(jobInstance.isActiveAndEnabled)
                jobInstance.doTask(oncePerTileCheck);
        }
    }

    public void addScript(string scriptToAdd)
    {
        Component newComponent = gameObject.AddComponent(Type.GetType(scriptToAdd));
        jobs.Add(newComponent);
    }

    public void removeScripts(string scriptsToRemove)
    {
        for(int i = 0; i < jobs.Count; i++)
        {
            if(jobs[i].GetType() == Type.GetType(scriptsToRemove))
            {
                DestroyImmediate(jobs[i] as JobBase);
                jobs.RemoveAt(i);
                i--;
            }
        }
    }

    public bool hasJob(Type type)
    {
        for(int i = 0; i < jobs.Count; i++)
        {
            if(jobs[i].GetType() == type)
            {
                return true;
            }
        }
        return false;
    }

    public JobBase getJob(Type type)
    {
        for (int i = 0; i < jobs.Count; i++)
        {
            if (jobs[i].GetType() == type)
            {
                return jobs[i] as JobBase; //Funktioniert as JobBase so?
            }
        }
        return null;
    }

    public void die()
    {
        //The only thing i could think of is that we have to tell the peepController to unregister our current position
        peepController.unregisterOldPosition(CustomUtil.Vector3ToInt(transform.position));
        Destroy(gameObject);
    }
    
}
