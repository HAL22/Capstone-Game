using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class GoldManager : MonoBehaviour {//used on heroes to monitor and control gold amounts

    public RectTransform goldBar;
    public Text goldText;
    public int maxGold = 100;

    private int goldAmount;
    private float barSize;

    // Use this for initialization
    void Start () {
        goldAmount = 0;
        barSize = goldBar.sizeDelta.y;
        updateGold();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addGold(int amount)
    {
        goldAmount = Mathf.Min(maxGold, goldAmount + amount);//cannot exceeed max gold amount
        updateGold();
    }

    public bool spendGold(int amount)//returns true if enough gold and gold is spent
    {
        if (amount > goldAmount)
        {
            return false;
        }
        else
        {
            goldAmount -= amount;
            updateGold();
            return true;
        }
    }

    private void updateGold()//updates visual gold bar on ui
    {
        goldBar.sizeDelta = new Vector2(goldBar.sizeDelta.x, (float)(goldAmount) / maxGold * barSize);
        goldText.text = goldAmount + "/" + maxGold; ;
    }

    public bool reachedMaxGold()//use to prevent picking up gold when at max gold
    {
        return goldAmount == maxGold;
    }
}
