using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectPath : MonoBehaviour {


    public void PathOne()
    {
        SceneManager.LoadScene("P1N1");
    }

    public void PathTwo()
    {
        SceneManager.LoadScene("P2N1");
    }

    public void PathThree()
    {
        SceneManager.LoadScene("P3N1");
    }

    public void PathFour()
    {
        SceneManager.LoadScene("P4N1");
    }
}
