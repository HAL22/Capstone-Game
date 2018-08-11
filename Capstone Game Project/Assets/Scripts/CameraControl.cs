using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraControl : MonoBehaviour
{

    // atrributes

    public Transform target;

    private Vector3 offset;

    private Transform trans;

	// Use this for initialization
	void Start ()
    {
        offset = new Vector3(0,10,-20);

        trans = GetComponent<Transform>();
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        
        if (target != null)
        {
            trans.position = target.position + offset;
            trans.LookAt(target); 
        }

		
	}

    public void SetTarget(GameObject player)
    {
        Debug.Log("Here");
        target = player.transform;

    }
}
