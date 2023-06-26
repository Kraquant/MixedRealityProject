using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaturationSlider : MonoBehaviour
{
	public Slider saturationSlider;
	public Slider hueSlider;
    public Image fillAreaImage;
    private float hue;
    
    private void Awake()
	{
  		saturationSlider = GetComponent<Slider>();
  		//fillAreaImage = saturationSlider.fillRect.GetComponentFromChildren<Image>();
	}
	
	private void Update()
	{
    	// Get the current value of the hue slider
    	hue = hueSlider.value;

    	// Calculate the gradient texture based on the hue
    	Texture2D saturationGradient = GenerateSaturationGradient(hue);

    	// Assign the gradient texture to the fill area image
    	fillAreaImage.sprite = Sprite.Create(saturationGradient, new Rect(0, 0, saturationGradient.width, saturationGradient.height), new Vector2(0.5f, 0.5f));
	}

	private Texture2D GenerateSaturationGradient(float hue)
	{
    	int width = 256; // Width of the gradient texture
    	int height = 1;  // Height of the gradient texture

    	Texture2D gradientTexture = new Texture2D(width, height);
    	gradientTexture.wrapMode = TextureWrapMode.Clamp;

    	Color[] colors = new Color[width];

    	for (int x = 0; x < width; x++)
    	{
        	float saturation = (float)x / (float)(width - 1); // Calculate the saturation based on the x position

        	// Convert the hue and saturation to RGB color
        	Color color = Color.HSVToRGB(hue, saturation, 1f);

        	colors[x] = color;
    	}

    	gradientTexture.SetPixels(colors);
    	gradientTexture.Apply();

    	return gradientTexture;
	}
}




