using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{

    //Private variables
    private int amountOfGold; // the amount of gold a minion has
    private bool collected;
    private LayerMask Resources; // where a empty gameobject is connected to the two heroes

    //Public variable
    public float rad;

    public void setAmount(int amount, LayerMask Layer)
    {
        amountOfGold = amount;
        collected = false;
        Resources = Layer;
    }

    

    private void Update()
    {
        if (collected)
        {
            Destroy(gameObject);
        }

        checkForHero();



        
    }

    void checkForHero()
    {
        Collider[] Heroes = Physics.OverlapSphere(transform.position, rad, Resources);

        if (Heroes.Length > 0)
        {
            

            if (Heroes[0].gameObject != null && Heroes[0].GetComponent<playerControl>() != null)
            {
                Heroes[0].gameObject.GetComponent<playerControl>().addGold(amountOfGold);
                collected = true;

            }
           
        }


    }


    


}
