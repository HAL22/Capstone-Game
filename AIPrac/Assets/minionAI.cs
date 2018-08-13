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



	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        raycastLayer = 1 << LayerMask.NameToLayer("PrimaryGameObjects");
        targetObject = null;
        enemies = new  List<GameObject>();
        cap = 1;
        
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        SearchForTarget();
        moveToTarget();
		
	}

    void SearchForTarget()
    {
        
        if (targetObject==null)
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

            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].GetComponent<minionAI>().getTargetNum() < cap)
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
}
