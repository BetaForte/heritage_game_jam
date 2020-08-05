using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitionBasic : MonoBehaviour
{
    public GameObject nextPage;
    public GameObject prevPage;
    public List<GameObject> customPages = new List<GameObject>();

    public GameObject loadingPage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextPage()
    {
        if(nextPage)
        {
            nextPage.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void PrevPage()
    {
        if (prevPage)
        {
            prevPage.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void CustomPage(int page)
    {
        if(customPages.Count > page && page >= 0 && customPages[page])
        {
            customPages[page].SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void On2PlayerStartButtonClicked()
    {
        if(loadingPage)
        {
            loadingPage.SetActive(true);
            gameObject.SetActive(false);
        }
        SceneManager.LoadScene(1);
    }

    public void On1PlayerStartButtonClicked()
    {
        if (loadingPage)
        {
            loadingPage.SetActive(true);
            gameObject.SetActive(false);
        }
        SceneManager.LoadScene(2);
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
}
