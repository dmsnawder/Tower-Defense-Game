using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseOptions : MonoBehaviour
{

    private PauseScript pauseScript;

    public void Resume()
    {
        pauseScript.Unpause();
    }

    public void Restart()
    {
        // TODO: Will probly need to change this
        pauseScript.Unpause();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        pauseScript.Unpause();
        GameObject fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
        GameObject clockLabel = GameObject.Find("ClockLabel");
        GameObject nightLabel = GameObject.Find("NightLabel");
        fadeScreen.GetComponent<Animator>().SetTrigger("fadeToMenu");
        clockLabel.GetComponent<Animator>().SetTrigger("fadeToMenu");
        nightLabel.GetComponent<Animator>().SetTrigger("fadeToMenu");
        StartCoroutine(FadeAndDestroyAssets());
    }

    void Start()
    {
        pauseScript = GameObject.Find("PauseLabel").GetComponent<PauseScript>();
    }

    IEnumerator FadeAndDestroyAssets()
    {
        yield return new WaitForSeconds(2f);

        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (GameObject hero in heroes)
        {
            Destroy(hero.gameObject);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        SceneManager.LoadScene(0);
    }

}
