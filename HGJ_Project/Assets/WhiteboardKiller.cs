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


    public void Killer(Score killer, GameObject killed)
    {
        StartCoroutine(KillerCoroutine(killer, killed));
    }

    private IEnumerator KillerCoroutine(Score killer, GameObject killed)
    {
        foreach (Text text in playerKilled)
        {
            text.gameObject.SetActive(true);
            text.text = killer.name + " has killed " + killed.name;
        }

        yield return new WaitForSeconds(5f);

        foreach (Text text in playerKilled)
        {
            text.gameObject.SetActive(false);
        }
    }

}
