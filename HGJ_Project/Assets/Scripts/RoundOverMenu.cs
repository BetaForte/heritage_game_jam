using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundOverMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAgain1P()
    {
        SceneManager.LoadScene(2);
    }
    public void PlayAgain2P()
    {
        SceneManager.LoadScene(1);
    }
    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
