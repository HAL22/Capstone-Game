using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class healthbarFaceCamera : MonoBehaviour {//used by healthbars on ui to always face the appropriate camera

    public Camera cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(cam.transform);
        transform.rotation = cam.transform.rotation;
    }
}
