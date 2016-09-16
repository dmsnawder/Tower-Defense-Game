using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HeroLevel
{
    public int level;
    public int cost;
    public int sellPrice;
    public GameObject attack;
    public GameObject visualization;
    public float attackRate;
}

public class HeroData : MonoBehaviour {

    public List<HeroLevel> levels;
    public int currentLevelIndex;
    public GameObject poofPrefab;
    public bool canAttack = true;
    float fadeInDuration = 0.5f;
    private float startTime;
    private float min = 0f;
    private float max = 1f;

    private HeroLevel currentLevel;
    public HeroLevel CurrentLevel
    {
        get { return currentLevel; }
        set
        {
            currentLevel = value;
            currentLevelIndex = levels.IndexOf(currentLevel);

            GameObject levelVisualization = levels[currentLevelIndex].visualization;
            for (int i = 0; i < levels.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentLevelIndex)
                    {
                        levels[i].visualization.SetActive(true);
                    }
                    else
                    {
                        levels[i].visualization.SetActive(false);
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        CurrentLevel = levels[0];
    }

    public HeroLevel GetNextLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        int maxLevel = levels.Count - 1;
        if (currentLevelIndex < maxLevel)
        {
            return levels[currentLevelIndex + 1];
        }
        else
        {
            return null;
            // TODO: make it so you can't click to upgrade it anymore with UI visualization and all that, instead of returning null.
        }
    }
    
    public void IncreaseLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = levels[currentLevelIndex + 1];
            gameObject.GetComponent<CircleCollider2D>().radius += 0.4f;
        }
    }

    void OnDestroy()
    {
        Instantiate(poofPrefab, gameObject.transform.position, Quaternion.identity);
    }

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float timeInterval = (Time.time - startTime) / fadeInDuration;
        CurrentLevel.visualization.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.SmoothStep(min, max, timeInterval));
    }

}
