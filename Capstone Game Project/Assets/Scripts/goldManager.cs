using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goldManager : MonoBehaviour {

    public RectTransform goldBar;
    public Text goldText;

    private int goldAmount;
    private int maxGold;
    private float barSize;

    // Use this for initialization
    void Start () {
        goldAmount = 0;
        barSize = goldBar.sizeDelta.x;
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
        goldBar.sizeDelta = new Vector2((float)(goldAmount) / maxGold * barSize, goldBar.sizeDelta.y);
        goldText.text = "" + goldAmount;
    }
}
