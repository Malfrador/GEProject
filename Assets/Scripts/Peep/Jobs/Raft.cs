using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raft : MonoBehaviour
{

    // Start is called before the first frame update
    private void Start()
    {
        Peep peepComponent = gameObject.GetComponent<Peep>();
        peepComponent.gameCoordinator.activateRaft();
        peepComponent.tileMap.SetTile(peepComponent.tileMap.WorldToCell(transform.position), peepComponent.tileController.basicBackgroundTile);
        peepComponent.removeScripts("Raft");
    }

}
