using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    private float speed;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {

        if (speed<=0.0f || target == null)
        {

            Destroy(gameObject);

        }

        Vector3 direction = target.position - transform.position;

        float distanceInFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceInFrame)
        {
            Destroy(gameObject);
        }


		
	}

    void SetData(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;
    }
}
