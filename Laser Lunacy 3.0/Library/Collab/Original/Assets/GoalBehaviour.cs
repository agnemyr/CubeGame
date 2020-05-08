using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    public GameObject completeLevelUI;
    public GameObject timerCanvas;

    private void Update()
    {
        if (Input.GetKeyDown("space"))        {            NextLevel();        }
    }

    public void WinGame()
    {
        
        // Time.timeScale = 0;
        completeLevelUI.SetActive(true);
        timerCanvas.SetActive(false);

        Invoke("PauseGame", 4f);
    }

    private void PauseGame ()
    {
        Time.timeScale = 0;
    }

    public void NextLevel ()
    {
        Debug.Log("Hello this is NextLevel()");
        
        foreach (GameBehaviour gb in FindObjectsOfType<GameBehaviour>())
        {
            Debug.Log(gb.isLocalPlayer);
            if (gb.isLocalPlayer)
            {
                gb.CmdLoadNextLevel();
            }
        }
    }

}
