using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScript : MonoBehaviour {
	
	public static bool paused = false;
    public GameObject pauseMenu;

    public AudioClip hoverClick;

    public void PlayClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(hoverClick);
    }

    void Start() {
		Paused = false;
	}

	void Update()
	{
		if(Input.GetButtonDown("Cancel"))
			paused = togglePause();
	}

    public void Pause()
    {
        Paused = true;
    }

    public void Unpause()
    {
        Paused = false;
    }

	public bool togglePause()
	{
		if(Time.timeScale == 0f)
		{
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            return false;
		}
		else
		{
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
			return true;
		}
	}
	
	public bool Paused {
		get {return paused;}
        set
        {
            paused = value;

            if (value == true)
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
            }
        }
	}

    public void Resume()
    {
        Unpause();
    }

    public void Restart()
    {
        // TODO: Will probly need to change this
        Unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Unpause();
        GameObject fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
        GameObject clockLabel = GameObject.Find("ClockLabel");
        GameObject nightLabel = GameObject.Find("NightLabel");
        fadeScreen.GetComponent<Animator>().SetTrigger("fadeToMenu");
        clockLabel.GetComponent<Animator>().SetTrigger("fadeToMenu");
        nightLabel.GetComponent<Animator>().SetTrigger("fadeToMenu");
        StartCoroutine(FadeAndDestroyAssets());
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
