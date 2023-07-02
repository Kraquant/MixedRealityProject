using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThicknessSlider : MonoBehaviour
{
	public Slider thicknessSlider;
	private int thickness;
	public Image paintImage;
	private float scale = 100f;
	
    private void Start()
    {
        thicknessSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Set the scale of the image based on the slider value
        paintImage.transform.localScale = new Vector3(value/scale, value/scale, 1f);
    }
}
