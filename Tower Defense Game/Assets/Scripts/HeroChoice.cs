using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroChoice : MonoBehaviour {

    public GameObject heroPrefab;
    public GameManager gameManager;
    public Sprite normal;
    public Sprite highlighted;
    private bool canPlace;
    public GUIStyle style;
    public bool isTutorial = false;
    public float openDuration = 0.5f;
    private float startTime;
    private float min = 0f;
    private float max = 1f;
    //public Font myFont;

    void OnMouseUp()
    {
        if (isTutorial)
        {
            if (gameObject.tag == "ArcherIcon")
            {
                transform.parent.GetComponent<PlaceOrUpgradeHero>().heroPrefab = heroPrefab;
                transform.parent.GetComponent<PlaceOrUpgradeHero>().Place();
            }
        }
        else if (canPlace)
        {
            transform.parent.GetComponent<PlaceOrUpgradeHero>().heroPrefab = heroPrefab;
            transform.parent.GetComponent<PlaceOrUpgradeHero>().Place();
        }
    }

    void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().sprite = highlighted;
    }

    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = normal;
    }

    void OnGUI()
    {
        style.fontSize = 18;
        //style.font = myFont;
        style.normal.textColor = Color.yellow;
        Vector3 getPixelPos = Camera.main.WorldToScreenPoint(transform.position);
        getPixelPos.y = Screen.height - getPixelPos.y;
        getPixelPos.x -= 20f;
        getPixelPos.y += 35f;

        GUI.Label(new Rect(getPixelPos.x, getPixelPos.y, 100f, 75f), heroPrefab.GetComponent<HeroData>().levels[0].cost + " G", style);
    }

    // Use this for initialization
    void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startTime = Time.time;

        GameObject tutorial = GameObject.Find("Tutorial");
        if (tutorial == null)
        {
            isTutorial = false;
        }
        else isTutorial = true;
    }

    void Update()
    {
        float timeInterval = (Time.time - startTime) / openDuration;
        transform.localScale = new Vector3(Mathf.SmoothStep(min, max, timeInterval), Mathf.SmoothStep(min, max, timeInterval), 1f);

        int cost = heroPrefab.GetComponent<HeroData>().levels[0].cost;
        if (gameManager.Gold >= cost)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            canPlace = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
            canPlace = false;
        }
    }
	
}
