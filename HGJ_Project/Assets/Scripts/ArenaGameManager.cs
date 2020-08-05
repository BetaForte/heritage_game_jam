using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArenaGameManager : MonoBehaviour
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

    private void Start()
    {
        if(!hasRoundStart)
        roundStartPanel.SetActive(true);

        roundTimeLeftPanel.SetActive(false);
        roundOverPanel.SetActive(false);
    }

    private void Update()
    {
        RoundStartUpdate();

        UpdateLeaderboard();

    }

    private void UpdateLeaderboard()
    {
        if (roundDurationTimer <= 0)
        {
            roundOverPanel.SetActive(true);
            isRoundOver = true;

            if (player1 != null)
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

            if (player2 != null)
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

            if (player3 != null)
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

            if (player4 != null)
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
            roundDurationTimer -= Time.deltaTime;
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


}
