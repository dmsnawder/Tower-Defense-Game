using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

    public GameObject tutorialScreen;
    public Text[] lessons;
    public GameManager gameManager;
    public SpawnEnemies spawnEnemies;
    private float timeSinceUnfrozen;
    private float unfreezeTime = 5f;
    private bool isFrozen = true;
    private bool unfrozen = false;
    public int lessonIndex = 0;
    public LayerMask whatToHit;

    public enum TutorialState
    {
        Goal,
        Lesson1,
        Lesson2,
        Lesson3,
        Lesson4,
        Lesson5,
        Lesson6
    }

    public TutorialState tutorialState;

    // Use this for initialization
    void Start () {
        tutorialState = TutorialState.Goal;
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        spawnEnemies = GameObject.Find("Path 1").GetComponent<SpawnEnemies>();

        timeSinceUnfrozen = Time.time;
        Freeze();
	}
	
	// Update is called once per frame
	void Update ()
    {
        State();
	}

    void State()
    {
        switch(tutorialState)
        {
            case TutorialState.Goal:
                GoalState();
                break;
            case TutorialState.Lesson1:
                Lesson1();
                break;
            case TutorialState.Lesson2:
                Lesson2();
                break;
            case TutorialState.Lesson3:
                Lesson3();
                break;
            case TutorialState.Lesson4:
                Lesson4();
                break;
            case TutorialState.Lesson5:
                Lesson5();
                break;
            case TutorialState.Lesson6:
                Lesson6();
                break;
        }
    }

    public void SkipTutorial()
    {
        UnFreeze();
        tutorialScreen.SetActive(false);
        Destroy(gameObject);
    }

    void GoalState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(GoalStateAction());
        }
    }

    IEnumerator GoalStateAction()
    {
        yield return new WaitForSeconds(0.2f);
        NextLesson();
        tutorialState = TutorialState.Lesson1;
    }

    void Lesson1()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = mousePos - position;
            float distance = Vector2.Distance(position, mousePos);
            Debug.DrawRay(position, dir, Color.yellow);

            RaycastHit2D hit = Physics2D.Raycast(position, dir, distance, whatToHit);
            if (hit)
            {
                print("There was a hit!");
                print(hit.collider.tag);
                if (hit.collider.tag == "TutorialSpot")
                {
                    StartCoroutine(CheckIfCorrectAction("Selection", TutorialState.Lesson2));
                }
            }
        }
    }

    void Lesson2()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = mousePos - position;
            float distance = Vector2.Distance(position, mousePos);
            Debug.DrawRay(position, dir, Color.yellow);

            RaycastHit2D hit = Physics2D.Raycast(position, dir, distance, whatToHit);
            if (hit)
            {
                print("There was a hit!");
                print(hit.collider.tag);
                if (hit.collider.tag == "ArcherIcon")
                {
                    StartCoroutine(CheckIfCorrectAction("Hero", TutorialState.Lesson3));
                }
            }
        }
    }

    void Lesson3()
    {
        if (isFrozen)
        {
            UnFreeze();
            unfreezeTime = 8f;
            tutorialScreen.SetActive(false);
            isFrozen = false;
            unfrozen = true;
        }

        float timeInterval = Time.time - timeSinceUnfrozen;

        if (timeInterval > unfreezeTime)
        {
            if (unfrozen)
            {
                Freeze();
                tutorialScreen.SetActive(true);
                unfrozen = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                Vector2 position = new Vector2(transform.position.x, transform.position.y);
                Vector2 dir = mousePos - position;
                float distance = Vector2.Distance(position, mousePos);
                Debug.DrawRay(position, dir, Color.yellow);

                RaycastHit2D hit = Physics2D.Raycast(position, dir, distance, whatToHit);
                if (hit)
                {
                    if (hit.collider.tag == "TutorialSpot")
                    {
                        StartCoroutine(CheckIfCorrectAction("Upgrade", TutorialState.Lesson4));
                    }
                }
            }
        }
    }
    
    IEnumerator CheckIfCorrectAction(string tag, TutorialState nextLesson)
    {
        yield return new WaitForSeconds(0.2f);

        GameObject obj = GameObject.FindGameObjectWithTag(tag);

        if (obj != null)
        {
            NextLesson();
            tutorialState = nextLesson;
            isFrozen = true;
            unfrozen = false;
        }

    }

    void Lesson4()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = mousePos - position;
            float distance = Vector2.Distance(position, mousePos);
            Debug.DrawRay(position, dir, Color.yellow);

            RaycastHit2D hit = Physics2D.Raycast(position, dir, distance, whatToHit);
            if (hit)
            {
                if (hit.collider.tag == "UpgradeIcon")
                {
                    StartCoroutine(CheckLesson4Action());
                }
            }
        }
    }

    IEnumerator CheckLesson4Action()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject hero = GameObject.FindGameObjectWithTag("Hero");

        if (hero.GetComponent<HeroData>().currentLevelIndex == 1)
        {
            NextLesson();
            tutorialState = TutorialState.Lesson5;
        }
    }

    void Lesson5()
    {
        if (isFrozen)
        {
            UnFreeze();
            unfreezeTime = 5f;
            tutorialScreen.SetActive(false);
            isFrozen = false;
            unfrozen = true;
        }

        float timeInterval = Time.time - timeSinceUnfrozen;

        if (timeInterval > unfreezeTime)
        {
            if (unfrozen)
            {
                Freeze();
                tutorialScreen.SetActive(true);
                unfrozen = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                Vector2 position = new Vector2(transform.position.x, transform.position.y);
                Vector2 dir = mousePos - position;
                float distance = Vector2.Distance(position, mousePos);
                Debug.DrawRay(position, dir, Color.yellow);

                RaycastHit2D hit = Physics2D.Raycast(position, dir, distance, whatToHit);
                if (hit)
                {
                    print("There was a hit!");
                    print(hit.collider.tag);
                    if (hit.collider.tag == "Enemy")
                    {
                        StartCoroutine(CheckLesson5Action());
                    }
                }
            }
        }
    }

    IEnumerator CheckLesson5Action()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<BecomeSelectedTarget>().IsSelectedTarget)
            {
                NextLesson();
                tutorialState = TutorialState.Lesson6;
                isFrozen = true;
                unfrozen = false;
                break;
            }
        }

        tutorialScreen.transform.FindChild("SkipTutorial").gameObject.SetActive(false);
    }

    void Lesson6()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UnFreeze();
            tutorialScreen.SetActive(false);
            Destroy(gameObject);
        }
    }

    void NextLesson()
    {
        lessons[lessonIndex].gameObject.SetActive(false);
        lessons[lessonIndex + 1].gameObject.SetActive(true);
        lessonIndex++;
    }

    void Freeze()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // disable all heroes ability to attack
        foreach (GameObject hero in heroes)
        {
            hero.GetComponent<HeroData>().canAttack = false;
        }

        // disable all enemies ability to move
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<MoveEnemy>().canMove = false;
        }

        // freeze Game Manager's clock timer
        gameManager.isTutorial = true;

        // disable Enemy Spawner's ability to spawn enemies and freeze spawn timer
        spawnEnemies.canSpawn = false;

    }

    void UnFreeze()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //enable all heroes ability to attack
        foreach (GameObject hero in heroes)
        {
            hero.GetComponent<HeroData>().canAttack = true;
        }

        //enable all enemies ability to move
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<MoveEnemy>().canMove = true;
        }

        //unfreeze Game Manager's clock timer
        gameManager.isTutorial = false;
        gameManager.prevHourTime = Time.time;

        //enable Enemy Spawner's ability to spawn enemies and unfreeze spawn timer
        spawnEnemies.canSpawn = true;
        spawnEnemies.lastSpawnTime = Time.time;

        timeSinceUnfrozen = Time.time;
    }
}
