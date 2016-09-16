using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

	void Remove()
    {
        Destroy(gameObject);
    }

    void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
