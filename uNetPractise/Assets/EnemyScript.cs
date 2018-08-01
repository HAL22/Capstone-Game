using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class EnemyScript : NetworkBehaviour
{

    private NavMeshAgent agent;
    private Transform myTransform;
    private LayerMask raycastLayer;
    private Transform target;
    private float raduis=100;
    public NetworkInstanceId playerId;
    private NetworkInstanceId tempId;

    void Start()
    {
       // Debug.Log("searching");

        agent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        raycastLayer = 1 << LayerMask.NameToLayer("Player");
        
    }

     void FixedUpdate()
    {
        SearchForTarget();
       MoveToTarget();
        
    }

    void SearchForTarget()
    {
      
        if (!isServer)
        {
            return;
        }

        if (target == null)
        {
            Collider[] hitCollider = Physics.OverlapSphere(myTransform.position, raduis, raycastLayer);

            Debug.Log("Size of array" + hitCollider.Length);
            

            /*if (hitCollider.Length > 0)
            {

                
                //int randomint = Random.Range(0, hitCollider.Length);
                Debug.Log(hitCollider.Length);
                if (hitCollider[0].GetComponent<NetworkIdentity>().netId != null)
                {
                    tempId = hitCollider[0].gameObject.GetComponent<NetworkIdentity>().netId;
                }
                
                if (playerId != tempId)
                {
                    target = hitCollider[0].transform;


                }
                
               
               
            }*/

            for (int i=0;i<hitCollider.Length;i++)
            {
                if (hitCollider[i].GetComponent<NetworkIdentity>() != null)
                {
                    tempId = hitCollider[i].gameObject.GetComponent<NetworkIdentity>().netId;

                    if (playerId != tempId)
                    {
                        target = hitCollider[i].transform;


                    }

                   

                }

            }

        }

    }

    void MoveToTarget()
    {
        if (target != null && isServer)
        {
            Debug.Log("searching");
            SetDestination(target);
        }

    }

    void SetDestination(Transform dest)
    {
        agent.SetDestination(dest.position);
    }











}
