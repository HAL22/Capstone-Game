using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour {

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
        goldAmount = Mathf.Min(maxGold, goldAmount + amount);
        updateGold();
    }

    public bool spendGold(int amount)
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

    private void updateGold()
    {
        goldBar.sizeDelta = new Vector2(goldBar.sizeDelta.x, (float)(goldAmount) / maxGold * barSize);
        goldText.text = goldAmount + "/" + maxGold; ;
    }
}
