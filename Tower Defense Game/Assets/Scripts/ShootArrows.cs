using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootArrows : MonoBehaviour {

    public List<GameObject> enemiesInRange;
    public float lastAttackTime;
    private HeroData heroData;
    public bool facingRight = true;
    public List<GameObject> targets;
    public GameObject arrowPrefab;
    public GameObject arrowFlippedPrefab;


    // Use this for initialization
    void Start () {
        enemiesInRange = new List<GameObject>();
        targets = new List<GameObject>();
        lastAttackTime = Time.time;
        heroData = gameObject.GetComponent<HeroData>();
    }

    // Update is called once per frame
    void Update() {
        if (heroData.canAttack)
        {
            float closestToGoal = float.MaxValue;

            if (heroData.CurrentLevel.level > 1)
            {
                foreach (GameObject enemy in enemiesInRange)
                {
                    if (targets.Count < heroData.CurrentLevel.level) targets.Add(enemy);
                }
                if (targets.Count != 0)
                {
                    if (Time.time - lastAttackTime > heroData.CurrentLevel.attackRate)
                    {
                        ShootMultiple(targets);
                        lastAttackTime = Time.time;
                    }
                }
            }

            else
            {
                GameObject target = null;
                foreach (GameObject enemy in enemiesInRange)
                {
                    if (enemy)
                    {
                        float distanceToGoal = enemy.GetComponent<MoveEnemy>().DistanceToGoal();

                        if (distanceToGoal < closestToGoal)
                        {
                            closestToGoal = distanceToGoal;
                            target = enemy;
                        }

                        if (enemy.GetComponent<BecomeSelectedTarget>().IsSelectedTarget)
                        {
                            target = enemy;
                        }
                    }
                }

                if (target != null)
                {
                    if (target.transform.position.x < transform.position.x && facingRight)
                    {
                        Flip();
                    }
                    else if (target.transform.position.x >= transform.position.x && !facingRight)
                    {
                        Flip();
                    }

                    if (Time.time - lastAttackTime > heroData.CurrentLevel.attackRate)
                    {
                        Shoot(target.GetComponent<Collider2D>());
                        lastAttackTime = Time.time;
                    }
                }
            }

            targets.Clear();
        }
    }

    void Shoot(Collider2D target)
    {
        GameObject newProjectile;
        Vector3 startPosition = gameObject.transform.FindChild("SpawnPoint").position;

        if (facingRight)
        {
            newProjectile = (GameObject)Instantiate(arrowPrefab, startPosition, Quaternion.identity);
        }
        else
        {
            newProjectile = (GameObject)Instantiate(arrowFlippedPrefab, startPosition, Quaternion.identity);
        }

        ArrowBehavior projectileData = newProjectile.GetComponent<ArrowBehavior>();

        projectileData.spawner = gameObject;
        projectileData.doLerp = true;
        projectileData.doSlerp = false;
        projectileData.target = target.gameObject;
        projectileData.startPosition = startPosition;
        //projectileData.rotation = facingRight ? 0f : -360f;

        heroData.CurrentLevel.visualization.GetComponent<Animator>().SetTrigger("shoot");
        // TODO: Play audio clip of shooting projectile
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
    }

    void ShootMultiple(List<GameObject> targets)
    {
        if (targets[0].transform.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
        else if (targets[0].transform.position.x >= transform.position.x && !facingRight)
        {
            Flip();
        }

        heroData.CurrentLevel.visualization.GetComponent<Animator>().SetTrigger("shoot");

        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);

        GameObject newProjectile;
        Vector3 startPosition = gameObject.transform.FindChild("SpawnPoint").position;
       
        for (int i = 0; i < targets.Count; i++)
        {
            if (facingRight)
            {
                newProjectile = (GameObject)Instantiate(arrowPrefab, startPosition, Quaternion.identity);
            }
            else
            {
                newProjectile = (GameObject)Instantiate(arrowFlippedPrefab, startPosition, Quaternion.identity);
            }

            ArrowBehavior projectileData = newProjectile.GetComponent<ArrowBehavior>();

            projectileData.spawner = gameObject;
            projectileData.doLerp = false;
            projectileData.doSlerp = true;
            projectileData.target = targets[i];
            projectileData.startPosition = startPosition;
            //projectileData.rotation = facingRight ? 0f : -180f;
        }

        //targets.Clear();
    }

    void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
        //targets.Remove(enemy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate += OnEnemyDestroy;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
            targets.Remove(other.gameObject);
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate -= OnEnemyDestroy;
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
