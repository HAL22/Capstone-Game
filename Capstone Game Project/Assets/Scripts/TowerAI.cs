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


    // Use this for initialization
    void Start ()
    {
        Enemies = new List<GameObject>();
        target = null;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackDelay)
        {

            GetTarget();

            if (target != null && target.GetComponent<healthManager>().currentHealth>0)
            {
                Shoot();
            }

            attackTimer = 0.0f;

        }

        

    }

    void Shoot()
    {
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
}
