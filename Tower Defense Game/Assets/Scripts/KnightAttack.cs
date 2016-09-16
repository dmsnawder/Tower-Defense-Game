using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnightAttack : MonoBehaviour {

    public List<GameObject> enemiesInRange;
    public float lastAttackTime;
    private HeroData heroData;
    public bool facingRight = false;

    // Use this for initialization
    void Start () {
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
                    Attack(target.GetComponent<Collider2D>());
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    void Attack(Collider2D target)
    {
        GameObject attackPrefab = heroData.CurrentLevel.attack;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = attackPrefab.transform.position.z;
        targetPosition.z = attackPrefab.transform.position.z;

        transform.FindChild("Level " + (heroData.currentLevelIndex + 1)).GetComponent<SpriteRenderer>().enabled = false;
        GameObject newProjectile = Instantiate(attackPrefab);
        newProjectile.transform.position = startPosition;
        SlashBehavior projectileData = newProjectile.GetComponent<SlashBehavior>();
        projectileData.target = target.gameObject;
        projectileData.spawner = gameObject;
        projectileData.startPosition = startPosition;

        if (facingRight)
        {
            if (target.GetComponent<MoveEnemy>().facingRight)
            {
                projectileData.targetPosition = new Vector3(targetPosition.x - 0.3f, targetPosition.y, targetPosition.z);
            }
            else
            {
                projectileData.targetPosition = new Vector3(targetPosition.x - 0.7f, targetPosition.y, targetPosition.z);
            }
        }
        else
        {
            if (target.GetComponent<MoveEnemy>().facingRight)
            {
                projectileData.targetPosition = new Vector3(targetPosition.x + 0.7f, targetPosition.y, targetPosition.z);
            }
            else
            {
                projectileData.targetPosition = new Vector3(targetPosition.x + 0.3f, targetPosition.y, targetPosition.z);
            }
        }
        
        projectileData.rotation = facingRight ? 0f : 193f;

        // TODO: Play audio clip of shooting projectile
        // TODO: Also maybe make an animation of shooting projectile
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
        GameObject sprite = gameObject.transform.FindChild("Level 1").gameObject;
        facingRight = !facingRight;
        Vector3 theScale = sprite.transform.localScale;
        theScale.x *= -1;
        sprite.transform.localScale = theScale;
    }
}
