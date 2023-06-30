using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRaycast : MonoBehaviour
{
    private bool held=false;
    public bool modeEnabled=true;

    // Update is called once per frame
    void Update()
    {
        if (!modeEnabled) return;
        if (!Input.GetMouseButton(0)) {
            held=false;
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GenericRaycastHelper.ProcessHits(Physics.RaycastAll(ray, GenericRaycastHelper.MAX_DISTANCE), !held);
        held = true;
        
    }
}
