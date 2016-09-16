using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaceOrUpgradeHero : MonoBehaviour {

    public GameObject heroPrefab;
    public GameObject hero;
    public GameObject heroSpawnParticles;
    private GameManager gameManager;
    private static GameObject clickedSpot;
    public float openDuration = 0.5f;
    private float startTime;
    private float min = 0f;
    private float max = 1f;

    public void Place()
    {
        Vector3 newPosition = new Vector3(clickedSpot.transform.position.x, clickedSpot.transform.position.y + 0.5f, clickedSpot.transform.position.z);
        Instantiate(heroSpawnParticles, newPosition, heroSpawnParticles.transform.rotation);

        hero = (GameObject)Instantiate(heroPrefab, clickedSpot.transform.position, Quaternion.identity);
        clickedSpot.GetComponent<OpenHeroUI>().hero = hero;
        gameManager.Gold -= hero.GetComponent<HeroData>().CurrentLevel.cost;
        clickedSpot.GetComponent<BoxCollider2D>().enabled = true;
        Destroy(gameObject);
    }

    public void Upgrade()
    {
        hero.GetComponent<HeroData>().IncreaseLevel();
        gameManager.Gold -= hero.GetComponent<HeroData>().CurrentLevel.cost;
        clickedSpot.GetComponent<BoxCollider2D>().enabled = true;
        Destroy(gameObject);
    }

    public void Sell()
    {
        gameManager.Gold += hero.GetComponent<HeroData>().CurrentLevel.sellPrice;
        clickedSpot.GetComponent<BoxCollider2D>().enabled = true;
        Destroy(hero.gameObject);
        hero = null;
        clickedSpot.GetComponent<OpenHeroUI>().hero = null;
        Destroy(gameObject);
    }

    void OnMouseUp()
    {
        clickedSpot.GetComponent<BoxCollider2D>().enabled = true;
        Destroy(gameObject);
    }

    public void SetClickedSpot(GameObject clicked)
    {
        clickedSpot = clicked;
    }

	// Use this for initialization
	void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startTime = Time.time;
    }

    void Update()
    {
        //float timeInterval = (Time.time - startTime) / openDuration;
        //transform.localScale = new Vector3(Mathf.SmoothStep(min, max, timeInterval), Mathf.SmoothStep(min, max, timeInterval), 1f);
    }
	
}
