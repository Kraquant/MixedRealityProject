using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* Handle draing from an object by pressing a button
 * How to set-up:
 * 1) Add a Button (TextMeshPro)
 * 2) On this button, add a Event Trigger component
 * 3) On this component, add the event types "Pointer Down" and "Pointer Up"
 * 4) Add this script to the button
 * 5) Link the GameObject from which the paint will be sprayed to the "origin" property of this script. The raycast will be drawn from the blue vector on the GameObject
 * 
 * NOTE: Having the BasicRaycast at the same time will generate conflicts if we are touching a paintable surface while holding the button. Another script should
 * make sure that only one raycast script is active at a time, by disabling the other ones.
 */

public class ButtonRaycast : MonoBehaviour,IUpdateSelectedHandler,IPointerDownHandler,IPointerUpHandler
{
    private bool isPressed = false;
    private bool previousPressed = false;

    public Transform origin;
    
    public void OnUpdateSelected(BaseEventData data)
    {
        if (isPressed)
        {
            GenericRaycastHelper.ProcessHits(Physics.RaycastAll(new Ray(origin.position, origin.TransformDirection(Vector3.forward)), GenericRaycastHelper.MAX_DISTANCE), !previousPressed);
            previousPressed = true;
        } else
            previousPressed = false;
    }


    public void OnPointerDown(PointerEventData data) => isPressed = true;
    public void OnPointerUp(PointerEventData data) => isPressed = false;

}
