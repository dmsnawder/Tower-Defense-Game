using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootMagic : MonoBehaviour {

    public List<GameObject> enemiesInRange;
    public float lastAttackTime;
    private HeroData heroData;
    public bool facingRight = true;
    public GameObject arcaneParticles;
    public GameObject fireballParticles;
    public GameObject frostParticles;
    public AudioClip fireballSound;
    public AudioClip spiritballSound;


	// Use this for initialization
	void Start ()
    {
        enemiesInRange = new List<GameObject>();
        lastAttackTime = Time.time;
        heroData = gameObject.GetComponent<HeroData>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (heroData.canAttack)
        {
            GameObject target = null;
            float closestToGoal = float.MaxValue;

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
	}

    void Shoot(Collider2D target)
    {
        GameObject attackPrefab = heroData.CurrentLevel.attack;
        Vector3 startPosition = gameObject.transform.FindChild("SpawnPoint").position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = attackPrefab.transform.position.z;
        targetPosition.z = attackPrefab.transform.position.z;

        //Instantiate effect particles for conjuring magic attack from staff
        if (heroData.currentLevelIndex == 0)
        {
            Instantiate(arcaneParticles, startPosition, Quaternion.identity);
            AudioSource.PlayClipAtPoint(fireballSound, transform.position);
        }
        else if (heroData.currentLevelIndex == 1)
        {
            Instantiate(fireballParticles, startPosition, Quaternion.identity);
            AudioSource.PlayClipAtPoint(fireballSound, transform.position);
        }
        else if (heroData.currentLevelIndex == 2)
        {
            Instantiate(frostParticles, startPosition, Quaternion.identity);
            AudioSource.PlayClipAtPoint(spiritballSound, transform.position);
        }

        //Instantiate the magic attack object
        GameObject newProjectile = Instantiate(attackPrefab);
        newProjectile.transform.position = startPosition;
        MageProjectileBehavior projectileData = newProjectile.GetComponent<MageProjectileBehavior>();
        projectileData.spawner = gameObject;
        projectileData.target = target.gameObject;
        projectileData.startPosition = startPosition;
        projectileData.targetPosition = targetPosition;

        // TODO: Play audio clip of shooting projectile
    }

    void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
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
