using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteboardKiller : MonoBehaviour
{
    public Text[] playerKilled;


    private void Start()
    {
        foreach(Text text in playerKilled)
        {
            text.gameObject.SetActive(false);
        }
    }


    public void Killer(string killer, string killed)
    {
        StartCoroutine(KillerCoroutine(killer, killed));
    }

    private IEnumerator KillerCoroutine(string killer, string killed)
    {
        foreach (Text text in playerKilled)
        {
            text.gameObject.SetActive(true);
            text.text = killer + " has killed " + killed;
        }

        yield return new WaitForSeconds(5f);

        foreach (Text text in playerKilled)
        {
            text.gameObject.SetActive(false);
        }
    }

}
