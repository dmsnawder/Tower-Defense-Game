using UnityEngine;
using System.Collections;

public class BecomeSelectedTarget : MonoBehaviour {
       
    private bool isSelectedTarget;
    public bool IsSelectedTarget
    {
        get { return isSelectedTarget; }
        set
        {
            isSelectedTarget = value;
            transform.FindChild("ChosenTargetIcon").GetComponent<SpriteRenderer>().enabled = value;
        }
    }

    void OnMouseUp()
    {
        ToggleMainTarget();
    }

    void ToggleMainTarget()
    {
        if (!isSelectedTarget)
        {
            GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in currentEnemies)
            {
                if (enemy.GetComponent<BecomeSelectedTarget>().IsSelectedTarget)
                {
                    enemy.GetComponent<BecomeSelectedTarget>().IsSelectedTarget = false;
                }
            }

            IsSelectedTarget = true;
            //transform.FindChild("ChosenTargetIcon").GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            IsSelectedTarget = false;
            //transform.FindChild("ChosenTargetIcon").GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }

}
