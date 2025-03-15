using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class GameOverController : MonoBehaviour
{
    List<string> playerNames = new List<string>();
    List<int> playerScores = new List<int>();
    [SerializeField] TMP_Text OddsLeaderBoard;
    [SerializeField] TMP_Text EvensLeaderBoard;
    GameObject GameOverTitle;

    [SerializeField] GameObject InputHighScoreMenu;
    [SerializeField] TMP_Text NewHighScoreText;
    [SerializeField] List<TMP_Text> FieldToSelect = new List<TMP_Text>(); //the pointer to the three text fields to represent letterSelect
    [SerializeField] GameObject SelectorUI;
    [SerializeField] int BaseScoreMod = 100;

    string[] alphabet;
    int[] letterSelect = new int[3]; //the current letter value of the 3 fields

    int passRank;
    int newScore;
    int curSelect = 0; //which of the 3 fields is currently selected
    bool inputing = false;
    // Start is called before the first frame update
    void Start() 
    {
        //PlayerPrefs.DeleteAll();
        InputHighScoreMenu.SetActive(false);

        alphabet = "A.B.C.D.E.F.G.H.I.J.K.L.M.N.O.P.Q.R.S.T.U.W.X.Y.Z".Split(".");
        for (int i = 0; i < alphabet.Length; i++)
        {
            print(alphabet[i]);
        }

        GameOverTitle = transform.Find("GameOver").gameObject;
        GameOverTitle.SetActive(false);

        for (int i = 1; i < 11; i++)
        {
            playerNames.Add(PlayerPrefs.GetString("R" + i.ToString() + "Name", "AAA"));
            playerScores.Add(PlayerPrefs.GetInt("R" + i.ToString() + "Score", BaseScoreMod * (13 - i)));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (inputing)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))            
            {
                letterSelect[curSelect]++;
                if (letterSelect[curSelect] > alphabet.Count() - 1)
                {
                    letterSelect[curSelect] = 0;
                }
                print("value of currently selected letter increasing:" + "field: " + curSelect + " Increased too " + letterSelect[curSelect] + "|" + alphabet[letterSelect[curSelect]]);
                FieldToSelect[curSelect].text = alphabet[letterSelect[curSelect]];
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                letterSelect[curSelect]--;
                if (letterSelect[curSelect] < 0)
                {
                    letterSelect[curSelect] = alphabet.Count() - 1;
                }
                print("value of currently selected letter decreasing:" + "field: " + curSelect + " decreased too " + letterSelect[curSelect] + "|" + alphabet[letterSelect[curSelect]]);
                FieldToSelect[curSelect].text = alphabet[letterSelect[curSelect]];
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
            {
                curSelect--;
                if (curSelect < 0)
                {
                    curSelect = 2;
                }
                print("Currently selected letter changing to " + curSelect);
                SelectorUI.transform.position = new Vector3(FieldToSelect[curSelect].transform.position.x, SelectorUI.transform.position.y, SelectorUI.transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                curSelect++;
                if (curSelect > 2)
                {
                    curSelect = 0;
                }
                print("Currently selected letter changing to " + curSelect);
                SelectorUI.transform.position = new Vector3(FieldToSelect[curSelect].transform.position.x, SelectorUI.transform.position.y, SelectorUI.transform.position.z);
            }
        }
    }

    public void inputHighScore()
    {
        inputing = true;
        //print("Opening Highscore");
        InputHighScoreMenu.SetActive(true);
        NewHighScoreText.text = newScore.ToString();
        print(playerScores.Count.ToString());
        for (int i = 9; i > 0; i--)
        {
            print("iteration " + i + ":" + playerNames[i] + ":" + playerScores[i]);
            if (i == passRank) { print("New Highscore slot");  break; }
            print("Replacing with " + i + ":" + playerNames[i - 1] + ":" + playerScores[i - 1]);
            playerNames[i] = playerNames[i - 1];
            playerScores[i] = playerScores[i - 1];
        }

    }

    public void ContinueGameOver()
    {
        print("Continuing");
        playerScores[passRank] = newScore;
        playerNames[passRank] = FieldToSelect[0].text + FieldToSelect[1].text + FieldToSelect[2].text;
        //print("Highscore Submitted");
        inputing = false;
        InputHighScoreMenu.SetActive(false);
        GameOverTitle.SetActive(true);
        //for(int i = )
        OddsLeaderBoard.text = "";
        EvensLeaderBoard.text = "";
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetInt("R" + (i + 1).ToString() + "Score", playerScores[i]);
            PlayerPrefs.SetString("R" + (i + 1).ToString() + "Name", playerNames[i]);
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


    public void startGameOver(int score)
    {
        //print("Starting gameover");
        GameOverTitle.SetActive(false);
        newScore = score;
        for (int i = 0; i < 10; i++)
        {
            if (score > playerScores[i])
            {
                passRank = i;
                //print("New Highscore");
                print(passRank.ToString());
                inputHighScore();
                return;
            }
        }
        OddsLeaderBoard.text = "";
        EvensLeaderBoard.text = "";
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                OddsLeaderBoard.text += (i + 1).ToString() + ":" + playerNames[i] + ") " + playerScores[i].ToString() + "\n \n";
            } else
            {
                EvensLeaderBoard.text += (i + 1).ToString() + ":" + playerNames[i] + ") " + playerScores[i].ToString() + "\n \n";
            }
        }
        GameOverTitle.SetActive(true);
    }
}
