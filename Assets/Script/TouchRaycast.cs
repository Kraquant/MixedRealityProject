using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRaycast : MonoBehaviour
{
    Painter p;
    private bool reset = false;
    // Start is called before the first frame update
    void Start()
    {
        p=GetComponent<Painter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount<0) {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                p.ContinusDraw(hit.point,reset || touch.phase == TouchPhase.Began);
                reset = false;
            }
            else
            {
                reset = true; // When we go outside the canvas, we don't want a line between the last point and the point where we entered again
            }
        }

    }
}
