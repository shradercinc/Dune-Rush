using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreConfirmation : MenuController
{
    [SerializeField] GameOverController GOCon;


    // Update is called once per frame
    void Update()
    {
        if (isOver && Input.GetKeyUp(KeyCode.Mouse0))
        {
            GOCon.ContinueGameOver();
        }
    }
}
