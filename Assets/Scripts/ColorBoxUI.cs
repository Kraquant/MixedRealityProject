using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorBoxUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image colorBoxImage;
    private bool isBoxSelected = false;

    public void UpdateColor(Color color)
    {
        colorBoxImage.color = color;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        isBoxSelected = !isBoxSelected;
    }
    
    public bool GetIsBoxSelected()
    {
    	return isBoxSelected;
    }
}
