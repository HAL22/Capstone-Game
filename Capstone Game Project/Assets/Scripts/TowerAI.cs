using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour
{

    public GameObject bullet;
    public Transform firePos;
    public GameObject target;
    public LayerMask EnemyLayer;
    public float searchRadius;
    public float attackRadius;
    public float attackDelay;
    public int healthImpact;
    public List<GameObject> Enemies;
    public float attackTimer;
    public GameObject deathEffect;

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
        if (state == State.alive)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > attackDelay)
            {

                GetTarget();

                if (target != null && target.GetComponent<healthManager>().getHealth() > 0)
                {
                    Shoot();
                }
                attackTimer = 0.0f;
            }

            checkDeath();
        }
        else
        {
            transform.position = transform.position - new Vector3(0, 1, 0);
        }

        

    }

    void Shoot()
    {
        GetComponent<AudioSource>().Play();
        GameObject shootBullet = (GameObject)Instantiate(bullet, firePos.position, firePos.rotation);
        shootBullet.GetComponent<Bullet>().SetData(target, 70.0f);
    }

    void GetTarget()
    {
        target = null;
        Enemies.Clear();
        // I check the specified radius for enemies
        Collider[] hitcollider = Physics.OverlapSphere(transform.position, searchRadius, EnemyLayer);

        if (hitcollider.Length > 0)
        {
            for (int i = 0; i < hitcollider.Length; i++)
            {
                Enemies.Add(hitcollider[i].gameObject);
            }

            if (Enemies.Count > 0)
            {
                Enemies.Sort(sortByidentity);
            }
        }


        for (int i = 0; i < Enemies.Count; i++)
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

    void checkDeath()
    {
        if (health.getHealth() <= 0) {
            state = State.dead;
            GameObject death = Instantiate(deathEffect, transform.position + new Vector3(0,2,0), transform.rotation);
        }
    }
}
