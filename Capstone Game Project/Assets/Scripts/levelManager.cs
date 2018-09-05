using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour {

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
