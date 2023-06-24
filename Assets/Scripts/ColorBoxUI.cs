using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorBoxUI : MonoBehaviour, IPointerClickHandler
{
	public Image paintSplashImage;
    [SerializeField] private Image colorBoxImage;
    public GameObject frame;
    private bool isBoxSelected = false;

    public void UpdateColor(Color color)
    {
        colorBoxImage.color = color;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        isBoxSelected = !isBoxSelected;
        frame.SetActive(isBoxSelected);
    }
    
    public bool GetIsBoxSelected()
    {
    	return isBoxSelected;
    }
    
    public void SetIsBoxSelected(bool value)
    {
    	isBoxSelected = value;
        frame.SetActive(isBoxSelected);
    }
}
