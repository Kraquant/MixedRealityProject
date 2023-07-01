using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is here to contain code that will be reused by all the kind of raycast types (mouse, touch, from object, ...)

public class GenericRaycastHelper
{
    
    private static Painter previous;
    public static readonly float MAX_DISTANCE = 100;
    private static bool forceReset = false;

    public static void ProcessHits(RaycastHit[] hits, bool reset=false) {
        float minDist = MAX_DISTANCE;
        RaycastHit closest = new RaycastHit(); // The new is here to make the compiler happy (otherwise, it would say that the variable is used while being non initialized)
        bool found = false;
        foreach (RaycastHit r in hits) {
            if (r.transform.GetComponent<Painter>() && r.distance < minDist) {
                closest = r;
                minDist = r.distance;
                found = true;
            }
        }
        if (found)
        {
            Painter p = closest.transform.GetComponent<Painter>();
            if (p != previous) {
                forceReset = true;
                previous = p;
            }
            p.ContinusDraw(closest,forceReset || reset);
            forceReset=false;
        }
        else
        {
            forceReset=true; // When we go outside the canvas, we don't want a line between the last point and the point where we entered again
        }
    }
}
