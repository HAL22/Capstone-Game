using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class levelManager : MonoBehaviour {//used to switch between scene and quit application

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void StartGame(string name)
    {
        SceneManager.LoadScene(name);
    }
}
