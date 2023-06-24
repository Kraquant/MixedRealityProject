using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicRaycast : MonoBehaviour
{
    private Camera cam;
    private bool held=false;
    public Ray Raycast { get; private set; }
    public RaycastHit RayHit { get; private set; }
    public bool Held { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0)) {
            held=false;
            return;
        }
        Raycast = cam.ScreenPointToRay(Input.mousePosition);
        Held = Physics.Raycast(Raycast, out RaycastHit rayHit, 100);
        RayHit = rayHit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Raycast.origin, Raycast.origin + 100.0f*Raycast.direction);
    }
}
