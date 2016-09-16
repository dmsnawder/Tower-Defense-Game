using UnityEngine;
using System.Collections;

public class DestroyParticles : MonoBehaviour {

    public float waitTime;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

}
