using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseListener : MonoBehaviour
{

    public MonoBehaviour[] disableOnPause;

    // Start is called before the first frame update
    private void Start()
    {
        GameCoordinator coordinator = FindObjectOfType<GameCoordinator>();
        coordinator.pauseEvent.AddListener(pauseGame);
    }

    public void pauseGame()
    {
        //Disable on pause is supposed to be set in the inspector
        if(disableOnPause != null)
        {
            foreach(MonoBehaviour toDisable in disableOnPause)
            {
                toDisable.enabled = !toDisable.enabled;
            }
        }
    }
}
