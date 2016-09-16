using UnityEngine;
using System.Collections;

public class ArrowBehavior : MonoBehaviour {

    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Vector3 targetPosition;

    public float speed = 10;
    public int damage;
    public GameObject target;
    public GameObject spawner;
    public bool doLerp = true;
    public bool doSlerp = false;
    public float rotation;
    public GameObject shadowHitParticles;
    public AudioClip arrowHitSound;

    private float startTime;
    private GameManager gameManager;
    private float ease = 5;

    // Use this for initialization
    void Start () {
        startTime = Time.time;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

        if (target != null)
        {
            Vector3 dir = target.transform.position - gameObject.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(angle - rotation) > 180)
            {
                if (angle > 0 && rotation < 0)
                {
                    rotation -= (360 - angle + rotation) / ease;
                }
                else if (rotation > 0 && angle < 0)
                {
                    rotation += (360 - angle + rotation) / ease;
                }
            }
            else if (angle < rotation)
            {
                rotation -= Mathf.Abs(rotation - angle) / ease;
            }
            else
            {
                rotation += Mathf.Abs(angle - rotation) / ease;
            }

            transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);

            if (doLerp)
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else if (doSlerp)
            {
                gameObject.transform.position = Vector3.Slerp(startPosition, target.transform.position, (Time.time - startTime) / 0.5f);
            }

        }
        else
        {
            if (spawner != null)
            {
                if (spawner.GetComponent<ShootArrows>().enemiesInRange.Count != 0)
                {
                    foreach (GameObject enemy in spawner.GetComponent<ShootArrows>().enemiesInRange)
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
            HealthBar healthBar = other.transform.FindChild("HealthBar").gameObject.GetComponent<HealthBar>();
            healthBar.currentHealth -= damage;

            AudioSource.PlayClipAtPoint(arrowHitSound, transform.position);

            if (healthBar.currentHealth <= 0)
            {
                gameManager.Gold += healthBar.killReward;
            }
            else
            {
                Instantiate(shadowHitParticles, new Vector3(other.transform.position.x, other.transform.position.y + 0.5f, 0), Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
