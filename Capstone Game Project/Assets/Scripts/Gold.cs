using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class Gold : MonoBehaviour
{

    //Private variables
    private int amountOfGold; // the amount of gold a minion has
    private bool collected;
    private LayerMask Layer; //  checks which hero to give the gold to
    private float len;
    private float speed;

    //Public variable
    public float rad;//radius at which gold can be picked up
   
    public void setAmount(int amount, LayerMask Layer)//set gold amount an which player layer can see it and pick it up
    {
        amountOfGold = amount;
        collected = false;
        this.Layer = Layer;
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

    void checkForHero()//checks to see if a hero on the right unit layer has collided with the gold bar
    {
        Collider[] Heroes = Physics.OverlapSphere(transform.position, rad, Layer);

        if (Heroes.Length > 0)
        {
            for(int i=0; i< Heroes.Length; i++)
            {
                if (Heroes[i].gameObject != null && Heroes[i].GetComponent<playerControl>() != null && !Heroes[i].gameObject.GetComponent<GoldManager>().reachedMaxGold())
                {//if there is still an object, its a player and they don't have max gold, give them gold
                    Heroes[i].gameObject.GetComponent<GoldManager>().addGold(amountOfGold);
                    collected = true;
                    GetComponent<AudioSource>().Play();
                    Destroy(gameObject, 0.5f);
                    break;
                }
            }

           
        }


    }

    void animateObject()//animates the object to rotate and 'bounce'
    {
        float y =  Mathf.PingPong(speed * Time.time, len);
        Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = pos;

        transform.Rotate(Vector3.up, 100*Time.deltaTime);
    }

   

    





}
