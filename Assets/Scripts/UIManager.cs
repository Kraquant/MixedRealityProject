using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Slider thicknessSlider;
    public Slider hueSlider;
    public Slider saturationSlider;
    public Slider intensitySlider;
    public Slider transparencySlider;

	private float thickness;
    private float hue;
    private float saturation;
    private float intensity;
    private float transparency;
    
    private Color currentColor;
    public ColorBoxUI colorBoxScript;
    public GameObject frame;

    private void Start()
    {
        // Set up the initial values of the sliders
        thicknessSlider.value = 70f;
        hueSlider.value = 0f;
        saturationSlider.value = 1f;
        intensitySlider.value = 1f;
        transparencySlider.value = 1f;
        
        thickness = thicknessSlider.value;
        hue = hueSlider.value;
        saturation = saturationSlider.value;
        intensity = intensitySlider.value;
        transparency = transparencySlider.value;
        
        // Set up the initial color
        //currentColor = Color.white;
        currentColor = Color.HSVToRGB(hue, saturation, intensity);

		//hide frame
        frame.SetActive(false);

        // Attach event handlers to the slider events
        thicknessSlider.onValueChanged.AddListener(OnThicknessSliderChanged);
        hueSlider.onValueChanged.AddListener(OnHueSliderChanged);
        saturationSlider.onValueChanged.AddListener(OnSaturationSliderChanged);
        intensitySlider.onValueChanged.AddListener(OnIntensitySliderChanged);
        transparencySlider.onValueChanged.AddListener(OnTransparencySliderChanged);
    }

    private void OnThicknessSliderChanged(float value)
    {
        thickness = value;
        // Update the color or perform any other necessary actions
    }
    
    private void OnHueSliderChanged(float value)
    {
        hue = value;
        // Update the hue component of the color
        currentColor = Color.HSVToRGB(value, saturation, intensity);
        colorBoxScript.UpdateColor(currentColor);
        
    }

    private void OnSaturationSliderChanged(float value)
    {
        saturation = value;
        // Update the saturation component of the color
        currentColor = Color.HSVToRGB(hue, value, intensity);
        colorBoxScript.UpdateColor(currentColor);
        
    }

    private void OnIntensitySliderChanged(float value)
    {
        intensity = value;
        // Update the intensity component of the color
        currentColor = Color.HSVToRGB(hue, saturation, value);
        colorBoxScript.UpdateColor(currentColor);
        
    }
    
    private void OnTransparencySliderChanged(float value)
    {
        transparency = value;
        // Update the transparency component of the color
        currentColor.a = value;
        colorBoxScript.UpdateColor(currentColor);
    }
    
    public void UpdateSlidersFromColor(Color color)
	{
    	// Convert RGB color to HSI
    	Color.RGBToHSV(color, out hue, out saturation, out intensity);
    
    	// Set the HSI slider values
    	hueSlider.value = hue;
    	saturationSlider.value = saturation;
    	intensitySlider.value = intensity;
    	
    	// Set the alpha slider value
    	transparencySlider.value = color.a;
	}
}
