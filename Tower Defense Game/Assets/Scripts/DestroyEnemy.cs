using UnityEngine;
using System.Collections;

public class DestroyEnemy : MonoBehaviour {

	void Destroy ()
    {
        Destroy(transform.parent.gameObject);
    }
}
