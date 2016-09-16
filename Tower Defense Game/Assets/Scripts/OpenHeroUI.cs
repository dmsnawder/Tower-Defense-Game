using UnityEngine;
using System.Collections;

public class OpenHeroUI : MonoBehaviour {

    public Sprite openSpot;
    public Sprite openSpotAngledRight;
    public Sprite openSpotAngledLeft;
    public GameObject heroSelectionPrefab;
    public GameObject upgradePrefab;
    public GameObject upgradeSidewaysPrefab;
    public GameObject hero;
    private PauseScript pauseScript;

    public GameManager gameManager;

    public bool CanPlaceHero()
    {
        return hero == null;
    }

    public bool CanUpgradeHero()
    {
        return hero != null;
    }
    
    void OnMouseUp()
    {
        if (!pauseScript.Paused)
        {
            if (CanPlaceHero())
            {

                if (transform.position.y > 1.5f)
                {
                    GameObject heroSelection = (GameObject)Instantiate(heroSelectionPrefab, new Vector3(transform.position.x, transform.position.y - 0.4f, -1), Quaternion.identity);
                    heroSelection.GetComponent<PlaceOrUpgradeHero>().SetClickedSpot(this.gameObject);   
                }
                else
                {
                    GameObject heroSelection = (GameObject)Instantiate(heroSelectionPrefab, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
                    heroSelection.GetComponent<PlaceOrUpgradeHero>().SetClickedSpot(this.gameObject);
                }

                GetComponent<SpriteRenderer>().sprite = null;
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else if (CanUpgradeHero())
            {
                GameObject upgrade;
                if (transform.position.y > 1.5f || transform.position.y < -2.5f)
                {
                    upgrade = (GameObject)Instantiate(upgradeSidewaysPrefab, new Vector3(transform.position.x, transform.position.y + 0.25f, -1), Quaternion.identity);
                }
                else
                {
                    upgrade = (GameObject)Instantiate(upgradePrefab, new Vector3(transform.position.x, transform.position.y + 0.25f, -1), Quaternion.identity);
                    
                }
                upgrade.GetComponent<PlaceOrUpgradeHero>().SetClickedSpot(this.gameObject);
                upgrade.GetComponent<PlaceOrUpgradeHero>().hero = hero;
                upgrade.transform.FindChild("UpgradeIcon").GetComponent<Upgrade>().hero = hero;
                upgrade.transform.FindChild("SellIcon").GetComponent<SellHero>().hero = hero;
                Transform range = upgrade.transform.FindChild("Range");
                range.localScale = new Vector3(hero.GetComponent<CircleCollider2D>().radius, hero.GetComponent<CircleCollider2D>().radius, 1);

                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    void OnMouseEnter()
    {
        if (!pauseScript.Paused)
        {
            if (hero == null)
            {
                if (transform.position.x < -2f)
                {
                    GetComponent<SpriteRenderer>().sprite = openSpotAngledRight;
                }
                else if (transform.position.x > 2f)
                {
                    GetComponent<SpriteRenderer>().sprite = openSpotAngledLeft;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = openSpot;
                }
            }
        }
    }

    void OnMouseExit()
    {
        if (hero == null)
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pauseScript = GameObject.Find("PauseLabel").GetComponent<PauseScript>();
    }
	
}
