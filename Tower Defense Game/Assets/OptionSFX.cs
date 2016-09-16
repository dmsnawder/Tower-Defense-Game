using UnityEngine;
using System.Collections;

public class OptionSFX : MonoBehaviour {

    public AudioClip hoverClick;

    public void PlayClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(hoverClick);
    }

}
