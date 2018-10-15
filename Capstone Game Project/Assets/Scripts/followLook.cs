using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class followLook : MonoBehaviour {//used by Camera 1 and 2 to 'chase' heroes

    public Transform target;
    public float speed = 0.15f;
    public float offset = 1.0f;
    public float height = 5f;

    private float radians;
    private float rotation;

	// Use this for initialization
	void Start () {
		
	}

    void FixedUpdate()
    {
        radians = rotation / 360 * 2 * Mathf.PI + Mathf.PI;
        rotation = target.rotation.eulerAngles.y;
        radians = rotation / 360 * 2 * Mathf.PI + Mathf.PI;
        Vector3 desiredPos = target.position + new Vector3(Mathf.Sin(radians) * offset, height, Mathf.Cos(radians) * offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, speed);
        transform.LookAt(target.position + new Vector3(0, 2.5f, 0));
    }
}
