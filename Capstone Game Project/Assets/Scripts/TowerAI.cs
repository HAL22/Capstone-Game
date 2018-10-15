using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class TowerAI : MonoBehaviour
{

    public GameObject bullet;
    public Transform firePos;//position bullet fires from
    public LayerMask EnemyLayer;
    public float searchRadius;
    public float attackRadius;
    public float attackDelay;
    public int healthImpact;
    public float attackTimer;
    public GameObject deathEffect;

    private GameObject target;
    private List<GameObject> Enemies;
    private enum State { alive, dead };
    private State state;
    private healthManager health;


    // Use this for initialization
    void Start ()
    {
        state = State.alive;
        Enemies = new List<GameObject>();
        target = null;
        health = GetComponent<healthManager>();
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.alive)//if tower still alive
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > attackDelay)//it attack cooldown passed
            {

                GetTarget();//find target

                if (target != null && target.GetComponent<healthManager>().getHealth() > 0)//shoot at target is there and not dead
                {
                    Shoot();
                }
                attackTimer = 0.0f;
            }

            checkDeath();//check if tower has died
        }
        else//if tower dead move it throguh the ground
        {
            transform.position = transform.position - new Vector3(0, 1, 0);
        }

        

    }

    void Shoot()//shoot bullet at target and play shoot noise
    {
        GetComponent<AudioSource>().Play();
        GameObject shootBullet = (GameObject)Instantiate(bullet, firePos.position, firePos.rotation);
        shootBullet.GetComponent<Bullet>().SetData(target, 70.0f);
    }

    void GetTarget()//look for target
    {
        target = null;
        Enemies.Clear();
        // I check the specified radius for enemies
        Collider[] hitcollider = Physics.OverlapSphere(transform.position, searchRadius, EnemyLayer);

        if (hitcollider.Length > 0)
        {
            for (int i = 0; i < hitcollider.Length; i++)//add all enemies in search radies to array
            {
                Enemies.Add(hitcollider[i].gameObject);
            }

            if (Enemies.Count > 0)//sort enemies by priority order
            {
                Enemies.Sort(sortByidentity);
            }
        }


        for (int i = 0; i < Enemies.Count; i++)//find first feesible target
        {
            if (Enemies[i] != null) // if the gameobject are not null
            {
                target = Enemies[i];
                break;
            }
        }



    }

    static int sortByidentity(GameObject m1, GameObject m2)//sorting priority targets
    {
        int m1ID = m1.GetComponent<gameObjectIdentity>().ID;
        int m2ID = m2.GetComponent<gameObjectIdentity>().ID;

        return m1ID.CompareTo(m2ID);
    }

    void checkDeath()// if tower is dead play explosion and set state to dead
    {
        if (health.getHealth() <= 0) {
            state = State.dead;
            GameObject death = Instantiate(deathEffect, transform.position + new Vector3(0,2,0), transform.rotation);
        }
    }
}
