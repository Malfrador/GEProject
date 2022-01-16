using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Peep>().die();
    }

}
