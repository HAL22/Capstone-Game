using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private GameObject target;
    private float speed;
    private Transform targetPos;
    private GameObject effect;


    public float health;
    public GameObject fireballEffect; // Will surround the bullet

	// Use this for initialization
	void Start ()
    {

        health = 10.0f;
        effect = Instantiate(fireballEffect, transform.position, transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {

        if (speed<=0.0f || target == null)
        {

            Destroy(gameObject);
            Destroy(effect);

        }

        Vector3 direction = target.transform.position - transform.position;

        float distanceInFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceInFrame)
        {
            Vector3 dir = target.transform.position - targetPos.position;

            if (dir.magnitude < 1.0f && target!=null)
            {
                target.GetComponent<healthManager>().Damage(10);
            }

            Destroy(gameObject);
            Destroy(effect, 0.5f);
        }

        transform.Translate(direction.normalized * distanceInFrame, Space.World);


		
	}

    public void SetData(GameObject target, float speed)
    {
        this.target = target;
        this.speed = speed;
        targetPos = target.transform;
    }
}
