using UnityEngine;
using System.Collections;

public class SlashBehavior : MonoBehaviour {

    public float speed = 10;
    public GameObject target;
    public int damage;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public GameObject spawner;
    public bool attacking = true;
    public bool retreating = false;
    public float rotation;
    public GameObject shadowHitParticles;

    private float distance;
    private bool couldNotAttack = false;

    private GameManager gameManager;
    private Animator anim;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = gameObject.GetComponent<Animator>();

        distance = Vector3.Distance(startPosition, targetPosition);
        speed += distance;

        anim.speed = 0.3f * speed / distance;
        anim.SetTrigger("Attack");
        anim.SetInteger("currentLevel", spawner.GetComponent<HeroData>().currentLevelIndex);
    }
	
	// Update is called once per frame
	void Update () {

        if (attacking)
        {
            if (target != null)
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (transform.position == targetPosition)
                {
                    attacking = false;

                    if (spawner != null)
                    {
                        if (spawner.GetComponent<HeroData>().currentLevelIndex == 0)
                        {
                            StartCoroutine(WaitToJumpBack(0.3f));
                        }
                        else if (spawner.GetComponent<HeroData>().currentLevelIndex == 1)
                        {
                            StartCoroutine(WaitToJumpBack(0.6f));
                        }
                        else if (spawner.GetComponent<HeroData>().currentLevelIndex == 2)
                        {
                            StartCoroutine(WaitToJumpBack(0.9f));
                        }
                    }
                    //retreating = true;
                }
            }
            else
            {
                targetPosition = gameObject.transform.position;
                attacking = false;
                retreating = true;
                anim.SetTrigger("Retreat");
                couldNotAttack = true;
            }
        }

        else if (retreating)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

            if (gameObject.transform.position == startPosition)
            {
                if (spawner != null)
                {
                    spawner.transform.FindChild("Level " + (spawner.GetComponent<HeroData>().currentLevelIndex + 1)).GetComponent<SpriteRenderer>().enabled = true;

                    if (couldNotAttack)
                    {
                        spawner.GetComponent<KnightAttack>().lastAttackTime = Time.time - spawner.GetComponent<KnightAttack>().lastAttackTime;
                    }
                }

                Destroy(gameObject);
            }
        }

        transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);

    }

    IEnumerator WaitToJumpBack(float time)
    {
        yield return new WaitForSeconds(time);
        retreating = true;
        anim.SetTrigger("Retreat");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            HealthBar healthBar = other.transform.FindChild("HealthBar").gameObject.GetComponent<HealthBar>();
            healthBar.currentHealth -= damage;

            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            //if (!audioSource.isPlaying)
            //{
                audioSource.PlayOneShot(audioSource.clip);
            //}

            if (healthBar.currentHealth <= 0)
            {
                gameManager.Gold += healthBar.killReward;
            }
            else
            {
                Instantiate(shadowHitParticles, new Vector3(other.transform.position.x, other.transform.position.y + 0.5f, 0), Quaternion.identity);
            }

        }
    }
}
