using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardView : MonoBehaviour
{
    List<string> playerNames = new List<string>();
    List<int> playerScores = new List<int>();
    [SerializeField] int BaseScoreMod = 100;
    [SerializeField] TMP_Text OddsLeaderBoard;
    [SerializeField] TMP_Text EvensLeaderBoard;

    // Start is called before the first frame update
    void Start()
    {
        ResetLeaderboard();
        gameObject.SetActive(false);
    }

    public void ResetLeaderboard()
    {
        playerNames.Clear();
        playerScores.Clear();
        for (int i = 1; i < 11; i++)
        {
            playerNames.Add(PlayerPrefs.GetString("R" + i.ToString() + "Name", "AAA"));
            playerScores.Add(PlayerPrefs.GetInt("R" + i.ToString() + "Score", 1100 - (i * BaseScoreMod)));
        }
        OddsLeaderBoard.text = "";
        EvensLeaderBoard.text = "";
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                OddsLeaderBoard.text += (i + 1).ToString() + ":" + playerNames[i] + ") " + playerScores[i].ToString() + "\n \n";
            }
            else
            {
                EvensLeaderBoard.text += (i + 1).ToString() + ":" + playerNames[i] + ") " + playerScores[i].ToString() + "\n \n";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
