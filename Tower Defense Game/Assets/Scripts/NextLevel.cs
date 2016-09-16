using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class NextLevel : MonoBehaviour {

    public AudioClip morningSounds;

    void PlayMorningSounds()
    {
        GetComponent<AudioSource>().PlayOneShot(morningSounds);
    }

	// Use this for initialization
	void LoadNextLevel () {
        string sceneName = SceneManager.GetActiveScene().name;
        string[] sceneID = sceneName.Split('N');
        int nightNum = 0;

        if (Int32.TryParse(sceneID[1], out nightNum))
        {
            print("Night number: " + nightNum);
            if(nightNum < 6)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            Debug.Log("Couldn't parse scene name.");
            SceneManager.LoadScene(0);
        }
    }

}
