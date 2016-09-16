using UnityEngine;
using System.Collections;

public class BGMusic : MonoBehaviour {

    public GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (gameManager.nightSurvived)
        {
            GetComponent<AudioSource>().Stop();
        }
        else if (gameManager.gameOver)
        {
            GetComponent<AudioSource>().Stop();
        }
	}
}
