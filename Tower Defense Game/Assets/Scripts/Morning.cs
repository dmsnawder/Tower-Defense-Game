using UnityEngine;
using System.Collections;

public class Morning : MonoBehaviour {

    public Sprite morningBackground;
    public Sprite morningClosetDoor;

    public GameManager gameManager;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (gameManager.ClockTime == 6)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = morningBackground;
            transform.FindChild("Closet Door Open").GetComponent<SpriteRenderer>().sprite = morningClosetDoor;
        }
	}
}
