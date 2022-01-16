using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private void Start()
    {
        //Set Animation Bool to true
        gameObject.GetComponent<Animator>().SetBool("lava", true);


        StartCoroutine(Animation());
        
    }
    private IEnumerator Animation()
    {

        //wait for the animation to finish
        gameObject.GetComponent<PeepMovement>().enabled = false;
        yield return new WaitForSecondsRealtime(2f);
        gameObject.GetComponent<Peep>().die();
    }
}
