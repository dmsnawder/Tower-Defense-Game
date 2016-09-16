using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public Text goldLabel;
    public Text nightLabel;
    public Text clockLabel;
    int timeBetweenHours = 80;
    public float prevHourTime;
    int dawn = 6;
    public bool nightSurvived = false;
    public bool gameOver = false;
    public Scene currentScene;
    public bool isTutorial = false;
    public AudioClip alarm;

    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            goldLabel.text = "Gold: " + gold;
        }
    }

    private int night;
    public int Night
    {
        get { return night; }
        set
        {
            night = value;
            nightLabel.text = "Night " + night;
        }
    }

    private int clockTime;
    public int ClockTime
    {
        get { return clockTime; }
        set
        {
            clockTime = value;
            clockLabel.text = clockTime + " AM";
        }
    }

    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            if (health <= 0 && !gameOver)
            {
                gameOver = true;

                GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
                for (int i = 0; i < heroes.Length; i++)
                {
                    Destroy(heroes[i]);
                }

                GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
                gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
                GameObject fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
                fadeScreen.GetComponent<Animator>().SetBool("gameOver", true);
                nightLabel.gameObject.GetComponent<Animator>().SetBool("gameOver", true);
                clockLabel.gameObject.GetComponent<Animator>().SetBool("gameOver", true);
            }
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        //Night = 0;
    }

    // Use this for initialization
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        currentScene = scene;
        if (currentScene.buildIndex != 0 && currentScene.buildIndex != 1)
        {
            string[] sceneID = scene.name.Split('N');
            int nightNum = 0;
            Int32.TryParse(sceneID[1], out nightNum);

            goldLabel = GameObject.Find("GoldLabel").GetComponent<Text>();
            nightLabel = GameObject.Find("NightLabel").GetComponent<Text>();
            clockLabel = GameObject.Find("ClockLabel").GetComponent<Text>();

            nightSurvived = false;
            gameOver = false;
            ClockTime = 12;
            Night = nightNum;
            Gold = 800;
            Health = 1;

            prevHourTime = Time.time;

            //Save level that the player has reached
            PlayerPrefs.SetInt("scene", scene.buildIndex);
            PlayerPrefs.Save();
        }

	}

    void Update ()
    {
        if (!isTutorial)
        {
            if (currentScene.buildIndex != 0 && currentScene.buildIndex != 1)
            {
                float timeInterval = Time.time - prevHourTime;

                if (!nightSurvived)
                {
                    if (timeInterval > timeBetweenHours)
                    {
                        if (ClockTime == 12)
                        {
                            ClockTime = 1;
                            prevHourTime = Time.time;
                        }
                        else if (ClockTime < dawn - 1)
                        {
                            ClockTime++;
                            prevHourTime = Time.time;
                        }
                        else if (ClockTime == dawn - 1 && !gameOver)
                        {
                            ClockTime++;
                            prevHourTime = Time.time;
                            //nightSurvived = true;
                            GetComponent<AudioSource>().PlayOneShot(alarm);

                            GameObject fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
                            fadeScreen.GetComponent<Animator>().SetBool("gameOver", true);
                            GameObject nightSurvivedLabel = GameObject.FindGameObjectWithTag("NightSurvived");
                            nightSurvivedLabel.GetComponent<Animator>().SetBool("nightSurvived", true);
                            clockLabel.gameObject.GetComponent<Animator>().SetBool("nightSurvived", true);
                            nightLabel.gameObject.GetComponent<Animator>().SetBool("gameOver", true);

                            GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
                            for (int i = 0; i < heroes.Length; i++)
                            {
                                Destroy(heroes[i]);
                            }

                        }

                    }
                }
            }
        }
    }

}
