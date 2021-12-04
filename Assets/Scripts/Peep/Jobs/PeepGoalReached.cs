using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepGoalReached : JobBase
{
    // We dont even need to wait for the doTask() 
    void Start()
    {
        gameObject.GetComponent<Peep>().peepController.unregisterOldPosition(CustomUtil.Vector3ToInt(transform.position + new Vector3(0.5f, 0.5f, 0)));
        Destroy(gameObject); //Can be called before setting the gameCoordinator.peepGoalReached() because Destroy only activates in the next "cycle"
        gameObject.GetComponent<Peep>().gameCoordinator.peepGoalReached();
    }

    //I'll put this here still. Incase doTask() somehow gets called before Start() but that really shouldnt be the case since we destroy the object in Start()
    //But bette safe than sorry
    override public void doTask(bool oncePerTileCheck)
    {
        
    }

}
