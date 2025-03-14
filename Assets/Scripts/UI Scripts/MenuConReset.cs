using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuConReset : MenuController
{
    [SerializeField] LeaderboardView MyLeaderboard;
    // Update is called once per frame
    void Update()
    {
        if (isOver && Input.GetKeyUp(KeyCode.Mouse0))
        {
            PlayerPrefs.DeleteAll();
            MyLeaderboard.ResetLeaderboard();
        }
    }
}
