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
    public int damage;
    public GameObject fireballEffect; // Will surround the bullet

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
            //Destroy(effect);

        }

        Vector3 direction = target.transform.position - transform.position;

        float distanceInFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceInFrame)
        {
            Vector3 dir = target.transform.position - targetPos.position;

            if (dir.magnitude < 1.0f && target!=null)
            {
                target.GetComponent<healthManager>().Damage(damage);
            }

            if (fireballEffect != null)
            {
                effect = Instantiate(fireballEffect, transform.position, transform.rotation);     
            }
            Destroy(effect, 0.75f);
            Destroy(gameObject);
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
