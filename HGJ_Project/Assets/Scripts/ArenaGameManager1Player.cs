using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaGameManager1Player : MonoBehaviour
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

    [Header("Round Time Left Panel")]
    public GameObject roundTimeLeftPanel;
    public Text roundTimeLeftText;

    [Header("Round Over Panel")]
    public GameObject roundOverPanel;
    public Text player1Name;
    public Text player2Name;
    public Text player3Name;
    public Text player4Name;
    public Text player1Score;
    public Text player2Score;
    public Text player3Score;
    public Text player4Score;
    public Text player1Win;
    public Text player2Win;
    public Text player3Win;
    public Text player4Win;

    [Header("Pause Panel")]
    public GameObject pausePanel;
    public bool pauseOpen;

    public List<Score> players = new List<Score>();


    public int highestScore;

    private void Start()
    {
        roundDurationTimer = ArenaSettings.instance.roundTime;

        if (!hasRoundStart)
            roundStartPanel.SetActive(true);

        roundTimeLeftPanel.SetActive(false);
        roundOverPanel.SetActive(false);

        AddPlayers();


    }

    private void Update()
    {
        RoundStartUpdate();

        UpdateLeaderboard();

        if (!pausePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
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

    private void AddPlayers()
    {
        if (player1 != null)
            players.Add(player1);
        if (player2 != null)
            players.Add(player2);
        if (player3 != null)
        {
            players.Add(player3);
        }
        if (player4 != null)
        {
            players.Add(player4);
        }

    }

    private void UpdateLeaderboard()
    {
        if (isRoundOver)
        {
            roundOverPanel.SetActive(true);

            if (player1 != null)
            {
                player1Name.gameObject.SetActive(true);
                player1Score.gameObject.SetActive(true);

                player1Name.text = player1.name;
                player1Score.text = player1.score.ToString();

            }
            else
            {
                player1Name.gameObject.SetActive(false);
                player1Score.gameObject.SetActive(false);
            }

            if (player2 != null)
            {
                player2Name.gameObject.SetActive(true);
                player2Score.gameObject.SetActive(true);

                player2Name.text = player2.name;
                player2Score.text = player2.score.ToString();

            }

            else
            {
                player1Name.gameObject.SetActive(false);
                player1Score.gameObject.SetActive(false);
            }

            if (player3 != null)
            {
                player3Name.gameObject.SetActive(true);
                player3Score.gameObject.SetActive(true);

                player3Name.text = player3.name;
                player3Score.text = player3.score.ToString();

            }

            else
            {
                player3Name.gameObject.SetActive(false);
                player3Score.gameObject.SetActive(false);
            }

            if (player4 != null)
            {
                player4Name.gameObject.SetActive(true);
                player4Score.gameObject.SetActive(true);

                player4Name.text = player4.name;
                player4Score.text = player4.score.ToString();

            }
            else
            {
                player4Name.gameObject.SetActive(false);
                player4Score.gameObject.SetActive(false);
            }

        }
    }

    private void RoundStartUpdate()
    {
        if (!hasRoundStart)
        {
            startRoundTimer -= Time.deltaTime;
            roundStartTimerText.text = "Round Starts In: " + Mathf.Round(startRoundTimer);
            if (startRoundTimer <= 0.5f)
            {
                StartCoroutine(RoundGo());
            }
        }

        if (hasRoundStart)
        {
            if (roundDurationTimer >= 0.5f)
            {
                roundDurationTimer -= Time.deltaTime;
            }

            if (roundDurationTimer <= 0.5f)
            {
                isRoundOver = true;

                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (players[j].score > players[i].score)
                        {
                            highestScore = players[j].score;
                        }
                    }
                }

                if (player1 != null)
                {
                    if (player1.score == highestScore)
                    {
                        player1Win.gameObject.SetActive(true);
                    }
                }

                if (player2 != null)
                {
                    if (player2.score == highestScore)
                    {
                        player2Win.gameObject.SetActive(true);
                    }
                }

                if (player3 != null)
                {
                    if (player3.score == highestScore)
                    {
                        player3Win.gameObject.SetActive(true);
                    }
                }

                if (player4 != null)
                {
                    if (player4.score == highestScore)
                    {
                        player4Win.gameObject.SetActive(true);
                    }
                }

            }
            roundTimeLeftPanel.SetActive(true);
            roundTimeLeftText.text = "Time Left: " + Mathf.Round(roundDurationTimer);
        }
    }

    private IEnumerator RoundGo()
    {
        roundStartTimerText.text = "GO!";
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

    private IEnumerator PauseMenuDelay()
    {
        yield return new WaitForEndOfFrame();
        pauseOpen = false;
    }

}
