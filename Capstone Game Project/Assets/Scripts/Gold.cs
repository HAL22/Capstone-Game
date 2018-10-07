using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{

    //Private variables
    private int amountOfGold; // the amount of gold a minion has
    private bool collected;
    private LayerMask Layer; //  checks which hero to give the gold to
    private float len;
    private float speed;
    private float cooldown = 1.0f;
    private float action1;
    private float action2;


    //Public variable
    public float rad;
   
    


    public void setAmount(int amount, LayerMask Layer,int team)
    {
        amountOfGold = amount;
        collected = false;
        this.Layer = Layer;
        //gameObject.layer = team;
        action1 = 1f;
        action2 = 0.0f;
    }

    private void Start()
    {
        len = 3;
        speed = 5;
    
    }



    private void Update()
    {
        if (!collected)
        {
            animateObject();
            checkForHero();
        }

     

    }

    void checkForHero()
    {
        Collider[] Heroes = Physics.OverlapSphere(transform.position, rad, Layer);

        if (Heroes.Length > 0)
        {
            for(int i=0; i< Heroes.Length; i++)
            {
                if (Heroes[i].gameObject != null && Heroes[i].GetComponent<playerControl>() != null && !Heroes[i].gameObject.GetComponent<GoldManager>().reachedMaxGold())
                {
                    Heroes[i].gameObject.GetComponent<GoldManager>().addGold(amountOfGold);
                    collected = true;
                    GetComponent<AudioSource>().Play();
                    Destroy(gameObject, 0.5f);
                    break;
                }
            }

           
        }


    }

    void animateObject()
    {

        //action1 += Time.deltaTime;
        //action2 += Time.deltaTime;

        //Vector3 posA = new Vector3(reset.position.x, reset.position.y + len, reset.position.z);

        //ector3 posB = reset.transform.position;

        //ector3 currentPos = transform.position;

        // currentPos = posA;

        /*f (action1 > cooldown)
         {
             currentPos = posA;
             action1 = 0.0f;
         }

         if (action2 > cooldown)
         {
             currentPos = posB;
             action2 = 0.0f;
         }

         if(currentPos == posA)
         {
             Debug.Log("current pos is pos A");
         }

         else if(currentPos == posB)
         {
             Debug.Log("Current pos is pos b");
         }*/

        //transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * speed);

        //currentPos = posB;

        //ansform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * speed);

        float y =  Mathf.PingPong(speed * Time.time, len);
        Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = pos;

        transform.Rotate(Vector3.up, 100*Time.deltaTime);





    }

   

    





}
