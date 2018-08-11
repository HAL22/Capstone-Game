using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class follow : MonoBehaviour {

    public Transform target;
    public float speed = 0.15f;
    public float offset = 1.0f;
    public float height = 5f;
    public Slider[] sliders;

    private float radians;
    private float rotation;

	// Use this for initialization
	void Start () {
		
	}

    void FixedUpdate()
    {
        height = sliders[0].value;
        offset = sliders[1].value;
        radians = rotation / 360 * 2 * Mathf.PI + Mathf.PI;
        rotation = target.rotation.eulerAngles.y;
        radians = rotation / 360 * 2 * Mathf.PI + Mathf.PI;
        Vector3 desiredPos = target.position + new Vector3(Mathf.Sin(radians) * offset, height, Mathf.Cos(radians) * offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, speed);
    }
}
