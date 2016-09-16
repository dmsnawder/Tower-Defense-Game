using UnityEngine;
using System.Collections;

public class SellHero : MonoBehaviour {

    public GameObject hero;
    public GUIStyle style;
    public bool isTutorial = false;
    public float openDuration = 0.5f;
    private float startTime;
    private float min = 0f;
    private float max = 1f;

    void OnMouseUp()
    {
        if (!isTutorial)
        {
            transform.parent.GetComponent<PlaceOrUpgradeHero>().Sell();
        }
    }

    void OnGUI()
    {
        if (hero != null)
        {
            style.fontSize = 20;
            //style.font = myFont;
            style.normal.textColor = Color.yellow;
            Vector3 getPixelPos = Camera.main.WorldToScreenPoint(transform.position);
            getPixelPos.y = Screen.height - getPixelPos.y;
            getPixelPos.x -= 15f;
            getPixelPos.y += 30f;

            GUI.Label(new Rect(getPixelPos.x, getPixelPos.y, 100f, 75f), hero.GetComponent<HeroData>().CurrentLevel.sellPrice + " G", style);
        }
    }

    void Start()
    {
        GameObject tutorial = GameObject.Find("Tutorial");
        startTime = Time.time;

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
    }

}
