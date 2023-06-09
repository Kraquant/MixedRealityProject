using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SprayButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image sprayEffect;
    public PaintModeToggle paintModeToggle;

    private void Start()
    {
        // Initially hide the image
        sprayEffect.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Show the image when the button is pressed
        // only if spray mode, not in finger mode.
        if (paintModeToggle.GetSprayMode()) sprayEffect.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Hide the image when the button is released
        sprayEffect.gameObject.SetActive(false);
    }
}

