using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : MonoBehaviour
{
    //We basically only have to 
    //Inform the tile controller that a sacrifice has just occured
    //Tell our peep compontent to kill ourself
    void Start()
    {
        Peep peepComponent = gameObject.GetComponent<Peep>();
        peepComponent.tileController.sacrificePeep(transform.position);
        peepComponent.die();
        
    }

    
}
