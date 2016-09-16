using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastTimeSlowed = 0;
    public float lastWaypointSwitchTime;
    public float speed = 1.0f;
    public float slowedSpeed = 0.7f;
    private float regularSpeed = 1.0f;
    public bool facingRight = true;
    public float slowTime = 5f;
    public GameManager gameManager;
    public bool canMove = true;

    // Use this for initialization
    void Start () {
        //lastWaypointSwitchTime = Time.time;
        lastTimeSlowed = Time.time - slowTime;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        regularSpeed = speed;
        transform.position = waypoints[0].transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (canMove)
        {
            Vector3 startPosition = waypoints[currentWaypoint].transform.position;
            Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;

            if (Time.time - lastTimeSlowed > slowTime)
            {
                speed = regularSpeed;
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);

            if (endPosition.x < startPosition.x && facingRight)
            {
                Flip();
            }
            else if (endPosition.x >= startPosition.x && !facingRight)
            {
                Flip();
            }

            if (gameObject.transform.position.x == endPosition.x && gameObject.transform.position.y == endPosition.y)
            {
                if (currentWaypoint < waypoints.Length - 2)
                {
                    currentWaypoint++;
                    //lastWaypointSwitchTime = Time.time;
                }
                else
                {
                    enabled = false;
                    gameManager.Health -= 1;
                }
            }
        }
	}

    public float DistanceToGoal()
    {
        float distance = 0;
        distance += Vector3.Distance(gameObject.transform.position, waypoints[currentWaypoint + 1].transform.position);

        for (int i = currentWaypoint + 1; i < waypoints.Length - 1; i++)
        {
            Vector3 startPosition = waypoints[i].transform.position;
            Vector3 endPosition = waypoints[i + 1].transform.position;
            distance += Vector3.Distance(startPosition, endPosition);
        }

        return distance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FrostOrb")
        {
            speed = slowedSpeed;
            lastTimeSlowed = Time.time;
        }
    }

    public void Flip()
    {
        GameObject sprite = gameObject.transform.FindChild("Sprite").gameObject;
        facingRight = !facingRight;
        Vector3 theScale = sprite.transform.localScale;
        theScale.x *= -1;
        sprite.transform.localScale = theScale;
    }

}
