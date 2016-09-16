using UnityEngine;
using System.Collections;

public class FlashyUIBGAnimation : MonoBehaviour {

    public float openDuration = 0.5f;
    private float startTime;
    private float min = 0f;
    private float max = 1f;

    // Use this for initialization
    void Start () {
        startTime = Time.time;
        if (gameObject.name == "Range")
        {
            max = transform.parent.GetComponent<PlaceOrUpgradeHero>().hero.GetComponent<CircleCollider2D>().radius;
        }
        else max = 1;
	}
	
	// Update is called once per frame
	void Update () {
        float timeInterval = (Time.time - startTime) / openDuration;
        transform.localScale = new Vector3(Mathf.SmoothStep(min, max, timeInterval), Mathf.SmoothStep(min, max, timeInterval), 1f);
    }
}
