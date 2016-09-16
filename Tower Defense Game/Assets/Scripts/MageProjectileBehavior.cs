using UnityEngine;
using System.Collections;

public class MageProjectileBehavior : MonoBehaviour {

    public float speed = 10;
    public float explodeRadius;
    public GameObject target;
    public int damage;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public GameObject spawner;
    public GameObject shadowHitParticles;

    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        explodeRadius = gameObject.GetComponent<CircleCollider2D>().radius * 3f;
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            if (spawner != null)
            {
                if (spawner.GetComponent<ShootMagic>().enemiesInRange.Count != 0)
                {
                    foreach (GameObject enemy in spawner.GetComponent<ShootMagic>().enemiesInRange)
                    {
                        if (enemy)
                        {
                            target = enemy;
                            break;
                        }
                    }
                }
                else Destroy(gameObject);
            }
            else Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.tag == "Enemy")
                {
                    HealthBar healthBar = hitColliders[i].transform.FindChild("HealthBar").gameObject.GetComponent<HealthBar>();
                    healthBar.currentHealth -= damage;

                    Instantiate(shadowHitParticles, new Vector3(other.transform.position.x, other.transform.position.y + 0.5f, 0), Quaternion.identity);

                    if (healthBar.currentHealth <= 0)
                    {
                        gameManager.Gold += healthBar.killReward;
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
