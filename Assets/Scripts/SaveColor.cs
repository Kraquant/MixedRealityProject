using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveColor : MonoBehaviour, IPointerClickHandler
{
    public Image colorImage; // Reference to the color image of the box
    public UIManager uiManager;

    private bool isTopBoxSelected = false;
    [SerializeField] private ColorBoxUI topColorBox;
    [SerializeField] private Image topColorBoxImage;

    public void OnPointerClick(PointerEventData eventData)
    {
    	isTopBoxSelected = topColorBox.GetIsBoxSelected();
        if (isTopBoxSelected)
        {
            // Save the color of the top box to this box
            colorImage.color = topColorBoxImage.color;
            topColorBox.SetIsBoxSelected(false);
        }
        else
        {
            // Set this box as the top box and store its color
            isTopBoxSelected = true; //this may not make sense. remove.
            //topBoxColor = colorImage.color;
            topColorBox.UpdateColor(colorImage.color);
            uiManager.UpdateSlidersFromColor(colorImage.color);
        }
    }
}

