using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class minionAI : MonoBehaviour
{

    private NavMeshAgent agent;
    private Transform myTransform;
    private GameObject targetObject;
    private LayerMask raycastLayer;
    public float rad;
    public string id;
    public int cap;
    private List<GameObject> enemies;
    private int targetNum;
    public GameObject towerPos;
    public string tower;
    public bool mustDie;
    public bool isAlive;



	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        raycastLayer = 1 << LayerMask.NameToLayer("PrimaryGameObjects");
        targetObject = towerPos;
        enemies = new  List<GameObject>();
        cap = 1;
        isAlive = true;
        
		
	}

    static int sortByIdentity(GameObject m1, GameObject m2)
    {

        return m1.GetComponent<Identify>().minionPriority.CompareTo(m2.GetComponent<Identify>().minionPriority);

    }
	
	// Update is called once per frame
	void Update ()
    {
        die();
        SearchForTarget();
        moveToTarget();
		
	}

    void SearchForTarget()
    {



        /*  if (targetObject.GetComponent<Identify>().id != tower)
          {
              if (targetObject.GetComponent<minionAI>().isAlive==false)
              {
                  targetObject = towerPos;

              }

          }*/

        if (targetObject == null)
        {
            targetObject = towerPos;
        }


        if (targetObject==null || targetObject.GetComponent<Identify>().id==tower )
        {

            // Debug.Log("Target is null");

            enemies.Clear();

            Collider[] hitCollider = Physics.OverlapSphere(myTransform.position, rad, raycastLayer);

            Debug.Log(hitCollider.Length);

            if (hitCollider.Length > 0)
            {

                // Debug.Log("Got hits");

                for (int i = 0; i < hitCollider.Length; i++)
                {
                    if (hitCollider[i].gameObject.GetComponent<Identify>().id != id)
                    {
                        enemies.Add(hitCollider[i].gameObject);
                        

                    }
                }
            }

            else
            {
                targetObject = towerPos;
            }

            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].GetComponent<minionAI>().getTargetNum() < cap || enemies.Count==1)
                    {
                        enemies[i].GetComponent<minionAI>().targetThis();
                        Debug.Log(" I have a target");
                        targetObject = enemies[i];
                        break;

                    }
                }

            }

        }
    }

    void moveToTarget()
    {
        if (targetObject != null)
        {
            setNav(targetObject.transform);
        }

    }

    void setNav(Transform trans)
    {
        agent.SetDestination(trans.position);
    }




    public void targetThis()
    {
        targetNum++;

    }

    public void releaseTarget()
    {
        targetNum--;
    }

    public int getTargetNum()
    {
        return targetNum;
    }

    void die()
    {

        if (mustDie == true)
        {
            isAlive = false;
            Destroy(gameObject, 1.2f);
        }

    }
}
