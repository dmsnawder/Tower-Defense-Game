using UnityEngine;
using System.Collections;

public class CameraResize : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        float targetAspect1 = 16.0f / 9.0f;
        float targetAspect2 = 4.0f / 3.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;

        if (windowAspect == targetAspect1)
        {
            Camera.main.orthographicSize = targetAspect1 * 2.1f;
        }
        else if (windowAspect == targetAspect2)
        {
            Camera.main.orthographicSize = targetAspect2 * 3.15f;
        }



/*
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
*/
	}

}
