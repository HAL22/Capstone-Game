using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{

    //Private variables
    private int amountOfGold; // the amount of gold a minion has

    public void setAmount(int amount)
    {
        amountOfGold = amount;
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Here");
        col.gameObject.GetComponentInParent<playerControl>().addGold(amountOfGold);
        Destroy(gameObject);
    }


}
