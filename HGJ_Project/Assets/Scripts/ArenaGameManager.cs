using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaGameManager : MonoBehaviour
{
    public float startRoundTimer;
    public float roundDurationTimer;

    public Score player1;
    public Score player2;
    public Score AI1;
    public Score AI2;

    public bool hasRoundStart;

    [Header("Round Start Panel")]
    public GameObject roundStartPanel;
    public Text roundStartTimerText;
    public Text roundStartTimerText2;

    private void Start()
    {
        if(!hasRoundStart)
        roundStartPanel.SetActive(true);
    }

    private void Update()
    {
        if(!hasRoundStart)
        {
            startRoundTimer -= Time.deltaTime;
            roundStartTimerText.text = "Round Starts In: " + Mathf.Round(startRoundTimer);
            roundStartTimerText2.text = "Round Starts In: " + Mathf.Round(startRoundTimer);
            if(startRoundTimer <= 0.5f)
            {
                StartCoroutine(RoundGo());
            }
        }

        if(hasRoundStart)
        {
            roundDurationTimer -= Time.deltaTime;
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


}
