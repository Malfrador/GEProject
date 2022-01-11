using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMarker : MonoBehaviour
{

    public GameCoordinator gameCoordinator;

    private void Start()
    {
        gameCoordinator.trapMarkersClear.AddListener(die);
    }

    private void OnMouseUpAsButton()
    {
        gameCoordinator.clearTrap(transform.position);
    }

    public void die()
    {
        Destroy(gameObject);
    }
}
