using UnityEngine;
using System.Collections;

public class EnemyDestructionDelegate : MonoBehaviour {

    public delegate void EnemyDelegate(GameObject enemy);
    public EnemyDelegate enemyDelegate;
    public GameObject poofPrefab;
    public AudioClip deathSound;

    void OnDestroy()
    {
        if (enemyDelegate != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Instantiate(poofPrefab, gameObject.transform.position, Quaternion.identity);
            enemyDelegate(gameObject);
        }
    }

}
