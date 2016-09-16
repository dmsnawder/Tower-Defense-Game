using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public float maxHealth = 100;
    public float currentHealth = 100;
    public int killReward = 50;
    private float originalScale;

    public GameManager gameManager;

	// Use this for initialization
	void Start () {
        originalScale = gameObject.transform.localScale.x;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gameManager.Night == 2)
        {
            maxHealth = Mathf.Floor(maxHealth * 1.1f);
            currentHealth = Mathf.Floor(currentHealth * 1.1f);
        }
        else if (gameManager.Night == 3)
        {
            maxHealth = Mathf.Floor(maxHealth * 1.15f);
            currentHealth = Mathf.Floor(currentHealth * 1.15f);
        }
        else if (gameManager.Night == 4)
        {
            maxHealth = Mathf.Floor(maxHealth * 1.2f);
            currentHealth = Mathf.Floor(currentHealth * 1.2f);
        }
        else if (gameManager.Night == 5)
        {
            maxHealth = Mathf.Floor(maxHealth * 1.25f);
            currentHealth = Mathf.Floor(currentHealth * 1.25f);
        }
        else if (gameManager.Night == 6)
        {
            maxHealth = Mathf.Floor(maxHealth * 1.3f);
            currentHealth = Mathf.Floor(currentHealth * 1.3f);
        }

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 tmpScale = gameObject.transform.localScale;
        tmpScale.x = currentHealth / maxHealth * originalScale;
        gameObject.transform.localScale = tmpScale;

        if (currentHealth <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
	}
}
