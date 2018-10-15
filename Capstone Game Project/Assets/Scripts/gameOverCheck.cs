using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class gameOverCheck : MonoBehaviour {//used to check if win/tie condition has been met

    public GameObject[] tower;//araay of towers (2)
    public Transform[] towerFocus;//used to set losing player camera to their tower to see it die
    public Text text;//game over text
    public Camera gameOverCam;//activated when game over
    public Camera [] playerCamera;//used in conjustion with tower focus to set losing camera
    public AudioSource gameMusic;
    public AudioClip victoryMusic;
    public GameObject endClick;//makes screen clickable to go back to main menu

    private bool over;

	// Use this for initialization
	void Start () {
        gameOverCam.enabled = false;
        endClick.SetActive(false);
        over = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!over)//while not over check if tower/s are destroyed
        {
            if(tower[0].GetComponent<healthManager>().getHealth() <=0 && tower[1].GetComponent<healthManager>().getHealth() <= 0)
            {//if game ends in a tie
                over = true;
                text.text = "Game Ends in a Tie!";
                gameOverCam.enabled = true;
                endClick.SetActive(true);
                playerCamera[0].gameObject.GetComponent<followLook>().target = towerFocus[0];
                playerCamera[1].gameObject.GetComponent<followLook>().target = towerFocus[1];
                playVictory();
            }
            else if(tower[1].GetComponent<healthManager>().getHealth() <= 0)
            {//if player 1 wins
                over = true;
                text.text = "Player 1 Wins!!!";
                gameOverCam.enabled = true;
                endClick.SetActive(true);
                playerCamera[1].gameObject.GetComponent<followLook>().target = towerFocus[1];
                playVictory();
            }
            else if (tower[0].GetComponent<healthManager>().getHealth() <= 0)
            {//player 2 wins
                over = true;
                text.text = "Player 2 Wins!!!";
                gameOverCam.enabled = true;
                endClick.SetActive(true);
                playerCamera[0].gameObject.GetComponent<followLook>().target = towerFocus[0];
                playVictory();
            }
        }

		
	}

    public void Restart(string name)//go back to main menu
    {
        SceneManager.LoadScene(name);
    }

    public void playVictory()//change music to victory music
    {
        gameMusic.clip = victoryMusic;
        gameMusic.Play();
    }
}
