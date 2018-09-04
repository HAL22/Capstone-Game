using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private GameObject target;
    private float speed;
    private Transform targetPos;

    public float health;

	// Use this for initialization
	void Start ()
    {

        health = 10.0f;
		
	}

    // Update is called once per frame
    void Update()
    {

        if (speed<=0.0f || target == null)
        {

            Destroy(gameObject);

        }

        Vector3 direction = target.transform.position - transform.position;

        float distanceInFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceInFrame)
        {
            Vector3 dir = target.transform.position - targetPos.position;

            if (dir.magnitude < 3.0f)
            {
                target.GetComponent<healthManager>().Damage(10);
            }

            Destroy(gameObject);
        }


		
	}

    public void SetData(GameObject target, float speed)
    {
        this.target = target;
        this.speed = speed;
        targetPos = target.transform;
    }
}
