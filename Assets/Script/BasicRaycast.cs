using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRaycast : MonoBehaviour
{
    Painter p;
    private bool held=false;
    // Start is called before the first frame update
    void Start()
    {
        p=GetComponent<Painter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0)) {
            held=false;
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            p.ContinusDraw(hit.point,!held);
            held=true;
        }
        else
        {
            held=false; // When we go outside the canvas, we don't want a line between the last point and the point where we entered again
        }
    }
}
