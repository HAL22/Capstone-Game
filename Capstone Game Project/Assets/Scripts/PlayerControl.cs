using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class PlayerControl : NetworkBehaviour
{

    //attributes

    private NavMeshAgent NavMeshAgent;

    // When the local player connects to the server

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        Camera.main.GetComponent<CameraControl>().SetTarget(gameObject);


        
    }

    // Use this for initialization
    void Start ()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
		
	}

    void clickToMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 1000))
            {
                NavMeshAgent.destination = hit.point;
            }

        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer) // prevents the local player from controlling the clone from another player
        {
            return;
        }

        clickToMove();

    }
}
