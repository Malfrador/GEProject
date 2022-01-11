using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReroute : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent myEvent;

    public void reroute()
    {
        Debug.Log("Hello");
        myEvent.Invoke();
    }
}
