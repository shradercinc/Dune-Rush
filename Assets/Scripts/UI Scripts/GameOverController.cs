using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverController : MonoBehaviour
{
    List<string> playerNames = new List<string>();
    List<int> playerScores = new List<int>();
    [SerializeField] TMP_Text OddsLeaderBoard;
    [SerializeField] TMP_Text EvensLeaderBoard;
    GameObject GameOverTitle;
    // Start is called before the first frame update
    void Start()
    {
        GameOverTitle = transform.Find("GameOver").gameObject;
        GameOverTitle.SetActive(false);
        for (int i = 1; i < 11; i++)
        { 
            playerNames.Add(PlayerPrefs.GetString("R" + i.ToString() + "Name", "AAA"));
            playerScores.Add(PlayerPrefs.GetInt("R" + i.ToString() + "Name", 0));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startGameOver(int score)
    {
        GameOverTitle.SetActive(true);
        OddsLeaderBoard.text = "";
        EvensLeaderBoard.text = "";
        for (int i = 1; i < 11; i++)
        {
            if (i % 2 != 0)
            {
                OddsLeaderBoard.text += i.ToString() + ":" + playerNames[i - 1] + ") " + playerScores[i - 1].ToString() + "\n \n";
            } else
            {
                EvensLeaderBoard.text += i.ToString() + ":" + playerNames[i - 1] + ") " + playerScores[i - 1].ToString() + "\n \n";
            }
        }
    }
}
