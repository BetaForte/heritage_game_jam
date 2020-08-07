using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArenaGameManager2Player : MonoBehaviour
{
    public float startRoundTimer;
    public float roundDurationTimer;

    public Score player1;
    public Score player2;
    public Score player3;
    public Score player4;

    public bool hasRoundStart;
    public bool isRoundOver;

    [Header("Round Start Panel")]
    public GameObject roundStartPanel;
    public Text roundStartTimerText;
    public Text roundStartTimerText2;

    [Header("Round Time Left Panel")]
    public GameObject roundTimeLeftPanel;
    public Text roundTimeLeftText;
    public Text roundTimeLeftText2;

    [Header("Round Over Panel")]
    public GameObject roundOverPanel;
    public Text[] player1Name;
    public Text[] player2Name;
    public Text[] player3Name;
    public Text[] player4Name;
    public Text[] player1Score;
    public Text[] player2Score;
    public Text[] player3Score;
    public Text[] player4Score;
    public Text[] player1Win;
    public Text[] player2Win;
    public Text[] player3Win;
    public Text[] player4Win;

    [Header("Pause Panel")]
    public GameObject pausePanel;

    public List<Score> players = new List<Score>();

    public bool pauseOpen;


    public int highestScore;

    bool isPlayingBGM = false;


    private void Start()
    {
        Time.timeScale = 1;
        isPlayingBGM = false;
        if (ArenaSettings.instance.player3Vehicle == "None")
        {
            player3.gameObject.SetActive(false);

        }

        if (ArenaSettings.instance.player4Vehicle == "None")
        {
            player4.gameObject.SetActive(false);
        }

        roundDurationTimer = ArenaSettings.instance.roundTime;

        if(!hasRoundStart)
        roundStartPanel.SetActive(true);

        roundTimeLeftPanel.SetActive(false);
        roundOverPanel.SetActive(false);

        AddPlayers();

    }

    private void Update()
    {
        RoundStartUpdate();

        UpdateLeaderboard();

        if(!pausePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();

            pauseOpen = true;
            StartCoroutine(PauseMenuDelay());
        }

        if (pausePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape) && !pauseOpen)
        {
            UnpauseGame();
        }

    }

    private IEnumerator PauseMenuDelay()
    {
        yield return new WaitForEndOfFrame();
        pauseOpen = false;
    }

    private void AddPlayers()
    {
        if(player1.gameObject.activeInHierarchy)
            players.Add(player1);
        if (player2.gameObject.activeInHierarchy)
            players.Add(player2);
        if(player3.gameObject.activeInHierarchy)
        {
            players.Add(player3);
        }
        if(player4.gameObject.activeInHierarchy)
        {
            players.Add(player4);
        }    

    }

    private void UpdateLeaderboard()
    {
        if (isRoundOver)
        {
            Time.timeScale = 0;
            if (!isPlayingBGM)
            {
                SoundManager.instance.bgm[0].Play();
                isPlayingBGM = true;
            }

            roundOverPanel.SetActive(true);

            if (player1.gameObject.activeInHierarchy)
            {
                player1Name[0].gameObject.SetActive(true);
                player1Name[1].gameObject.SetActive(true);
                player1Score[0].gameObject.SetActive(true);
                player1Score[1].gameObject.SetActive(true);

                player1Name[0].text = player1.name;
                player1Name[1].text = player1.name;
                player1Score[0].text = player1.score.ToString();
                player1Score[1].text = player1.score.ToString();

            }
            else
            {
                player1Name[0].gameObject.SetActive(false);
                player1Name[1].gameObject.SetActive(false);
                player1Score[0].gameObject.SetActive(false);
                player1Score[1].gameObject.SetActive(false);
            }

            if (player2.gameObject.activeInHierarchy)
            {
                player2Name[0].gameObject.SetActive(true);
                player2Name[1].gameObject.SetActive(true);
                player2Score[0].gameObject.SetActive(true);
                player2Score[1].gameObject.SetActive(true);

                player2Name[0].text = player2.name;
                player2Name[1].text = player2.name;
                player2Score[0].text = player2.score.ToString();
                player2Score[1].text = player2.score.ToString();

            }

            else
            {
                player1Name[0].gameObject.SetActive(false);
                player1Name[1].gameObject.SetActive(false);
                player1Score[0].gameObject.SetActive(false);
                player1Score[1].gameObject.SetActive(false);
            }

            if (player3.gameObject.activeInHierarchy)
            {
                player3Name[0].gameObject.SetActive(true);
                player3Name[1].gameObject.SetActive(true);
                player3Score[0].gameObject.SetActive(true);
                player3Score[1].gameObject.SetActive(true);

                player3Name[0].text = player3.name;
                player3Name[1].text = player3.name;
                player3Score[0].text = player3.score.ToString();
                player3Score[1].text = player3.score.ToString();

            }

            else
            {
                player3Name[0].gameObject.SetActive(false);
                player3Name[1].gameObject.SetActive(false);
                player3Score[0].gameObject.SetActive(false);
                player3Score[1].gameObject.SetActive(false);
            }

            if (player4.gameObject.activeInHierarchy)
            {
                player4Name[0].gameObject.SetActive(true);
                player4Name[1].gameObject.SetActive(true);
                player4Score[0].gameObject.SetActive(true);
                player4Score[1].gameObject.SetActive(true);

                player4Name[0].text = player4.name;
                player4Name[1].text = player4.name;
                player4Score[0].text = player4.score.ToString();
                player4Score[1].text = player4.score.ToString();

            }
            else
            {
                player4Name[0].gameObject.SetActive(false);
                player4Name[1].gameObject.SetActive(false);
                player4Score[0].gameObject.SetActive(false);
                player4Score[1].gameObject.SetActive(false);
            }

        }
    }

    private void RoundStartUpdate()
    {
        if (!hasRoundStart)
        {
            startRoundTimer -= Time.deltaTime;
            roundStartTimerText.text = "Round Starts In: " + Mathf.Round(startRoundTimer);
            roundStartTimerText2.text = "Round Starts In: " + Mathf.Round(startRoundTimer);
            if (startRoundTimer <= 0.5f)
            {
                StartCoroutine(RoundGo());
            }
        }

        if (hasRoundStart)
        {
            if(roundDurationTimer >= 0.5f)
            {
                roundDurationTimer -= Time.deltaTime;
            }

            if(roundDurationTimer <= 0.5f)
            {
                isRoundOver = true;

                for(int i = 0; i < players.Count; i++)
                {
                    for(int j = 0; j < players.Count; j++)
                    {
                        if(players[j].score > players[i].score)
                        {
                            highestScore = players[j].score;
                        }
                    }
                }

                if(player1.gameObject.activeInHierarchy)
                {
                    if (player1.score == highestScore)
                    {
                        player1Win[0].gameObject.SetActive(true);
                        player1Win[1].gameObject.SetActive(true);
                    }
                }

                if(player2.gameObject.activeInHierarchy)
                {
                    if (player2.score == highestScore)
                    {
                        player2Win[0].gameObject.SetActive(true);
                        player2Win[1].gameObject.SetActive(true);
                    }
                }

                if(player3.gameObject.activeInHierarchy)
                {
                    if (player3.score == highestScore)
                    {
                        player3Win[0].gameObject.SetActive(true);
                        player3Win[1].gameObject.SetActive(true);
                    }
                }

                if(player4.gameObject.activeInHierarchy)
                {
                    if (player4.score == highestScore)
                    {
                        player4Win[0].gameObject.SetActive(true);
                        player4Win[1].gameObject.SetActive(true);
                    }
                }

            }
            roundTimeLeftPanel.SetActive(true);
            roundTimeLeftText.text = "Time Left: " + Mathf.Round(roundDurationTimer);
            roundTimeLeftText2.text = "Time Left: " + Mathf.Round(roundDurationTimer);
        }
    }

    private IEnumerator RoundGo()
    {
        roundStartTimerText.text = "GO!";
        roundStartTimerText2.text = "GO!";
        yield return new WaitForSeconds(1f);

        hasRoundStart = true;
        roundStartPanel.SetActive(false);
    }

    public void OnPlayAgainButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);

        Time.timeScale = 0f;

    }

    public void UnpauseGame()
    {
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }


}
