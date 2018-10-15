using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class Bullet : MonoBehaviour //projectile object script
{

    private GameObject target;
    private float speed;
    private Transform targetPos;
    private GameObject effect;//local instance of fireballEffect

    public int damage;
    public GameObject fireballEffect; // particle effect on death

	// Use this for initialization
	void Start ()
    {  

    }

    // Update is called once per frame
    void Update()
    {

        if (speed<=0.0f || target == null)//destroy if stopped or target is gone
        {
            Destroy(gameObject);
        }



        Vector3 direction = target.transform.position - transform.position;

        float distanceInFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceInFrame)//if reached target
        {
            Vector3 dir = target.transform.position - targetPos.position;

            if (dir.magnitude < 1.0f && target!=null)//if hit target
            {
                target.GetComponent<healthManager>().Damage(damage);
            }

            if (fireballEffect != null)//if bullet has a death effect
            {
                effect = Instantiate(fireballEffect, transform.position, transform.rotation);     
            }
            Destroy(effect, 0.75f);
            Destroy(gameObject);
        }

        transform.Translate(direction.normalized * distanceInFrame, Space.World);//move bullet


		
	}

    public void SetData(GameObject target, float speed)//sets data when bullet instantiated
    {
        this.target = target;
        this.speed = speed;
        targetPos = target.transform;
    }
}
