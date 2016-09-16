using UnityEngine;
using System.Collections;

public class Upgrade : MonoBehaviour {

    public GameManager gameManager;
    public GameObject hero;
    private bool canUpgrade;
    public Sprite normalUpgradeIcon;
    public Sprite maxLevelIcon;
    public Sprite cantUpgradeIcon;
    public GUIStyle style;
    public GameObject upgradeParticles;
    public float openDuration = 0.5f;
    private float startTime;
    private float min = 0f;
    private float max = 1f;
    //public Font myFont;

    void OnMouseUp()
    {
        if (canUpgrade)
        {
            transform.parent.GetComponent<PlaceOrUpgradeHero>().Upgrade();
            Vector3 newPosition = new Vector3(transform.parent.position.x, transform.parent.position.y - 0.2f, transform.parent.position.z);
            Instantiate(upgradeParticles, newPosition, upgradeParticles.transform.rotation);
        }
    }

    void OnGUI()
    {
        if (hero != null)
        {
            if (hero.GetComponent<HeroData>().GetNextLevel() != null)
            {
                style.fontSize = 20;
                //style.font = myFont;
                style.normal.textColor = Color.yellow;
                Vector3 getPixelPos = Camera.main.WorldToScreenPoint(transform.position);
                getPixelPos.y = Screen.height - getPixelPos.y;
                getPixelPos.x -= 15f;
                getPixelPos.y += 30f;

                GUI.Label(new Rect(getPixelPos.x, getPixelPos.y, 100f, 75f), hero.GetComponent<HeroData>().GetNextLevel().cost + " G", style);
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startTime = Time.time;
    }

    void Update()
    {
        float timeInterval = (Time.time - startTime) / openDuration;
        transform.localScale = new Vector3(Mathf.SmoothStep(min, max, timeInterval), Mathf.SmoothStep(min, max, timeInterval), 1f);

        HeroLevel nextLevel = hero.GetComponent<HeroData>().GetNextLevel();

        if (nextLevel != null)
        {
            if (gameManager.Gold >= nextLevel.cost)
            {
                GetComponent<SpriteRenderer>().sprite = normalUpgradeIcon;
                canUpgrade = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = cantUpgradeIcon;
                canUpgrade = false;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = maxLevelIcon;
            canUpgrade = false;
        }
    }
	
}
