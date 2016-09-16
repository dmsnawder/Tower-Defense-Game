using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuOptions : MonoBehaviour {

    public GameObject newGameWarningBox;
    public AudioClip hoverClick;
    public AudioClip startGame;

    public void StartGame()
    {
        int savedScene = 0;
        savedScene = PlayerPrefs.GetInt("scene");

        if (savedScene == 0)
        {
            GameObject.Find("MenuFadeScreen").GetComponent<Animator>().SetTrigger("StartGame");
            GetComponent<AudioSource>().PlayOneShot(startGame);
            StartCoroutine(LetFadeOut(1));
        }
        else
        {
            newGameWarningBox.SetActive(true);
        }

    }

    //used by new game warning box
    public void ReallyStartGame()
    {
        CloseWarningBox();
        GameObject.Find("MenuFadeScreen").GetComponent<Animator>().SetTrigger("StartGame");
        GetComponent<AudioSource>().PlayOneShot(startGame);
        StartCoroutine(LetFadeOut(1));
    }

    public void Continue()
    {
        int savedScene = 0;
        savedScene = PlayerPrefs.GetInt("scene");
        
        if (savedScene != 0)
        {
            GameObject.Find("MenuFadeScreen").GetComponent<Animator>().SetTrigger("StartGame");
            GetComponent<AudioSource>().PlayOneShot(startGame);
            StartCoroutine(LetFadeOut(savedScene));
        }
    }

    public void CloseWarningBox()
    {
        newGameWarningBox.SetActive(false);
    }

    IEnumerator LetFadeOut(int scene)
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(scene);
    }

    public void PlayClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(hoverClick);
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("scene") == 0)
        {
            GameObject continueButton = transform.FindChild("Continue").gameObject;
            continueButton.transform.FindChild("Text").GetComponent<Text>().color = Color.gray;
            continueButton.GetComponent<Button>().enabled = false;
        }
    }
}
