using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntensitySlider : MonoBehaviour
{
	public Slider intensitySlider;
	public Slider hueSlider;
	public Image fillAreaImage;
	private float hue;
	
    private void Awake()
    {
  		intensitySlider = GetComponent<Slider>();
  		fillAreaImage = intensitySlider.fillRect.GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Retrieve the hue value from the HueSlider script
    	hue = hueSlider.value;

    	
    	// Calculate the gradient texture based on the hue
    	Texture2D intensityGradient = GenerateIntensityGradient(hue);

    	// Assign the gradient texture to the fill area image
    	fillAreaImage.sprite = Sprite.Create(intensityGradient, new Rect(0, 0, intensityGradient.width, intensityGradient.height), new Vector2(0.5f, 0.5f));
    }
    
    private Texture2D GenerateIntensityGradient(float hue)
	{
    	int width = 256; // Width of the gradient texture
    	int height = 1;  // Height of the gradient texture

    	Texture2D gradientTexture = new Texture2D(width, height);
    	gradientTexture.wrapMode = TextureWrapMode.Clamp;

    	Color[] colors = new Color[width];

    	for (int x = 0; x < width; x++)
    	{
        	float intensity = (float)x / (float)(width - 1); // Calculate the intensity based on the x position

        	// Convert the hue and intensity to RGB color
        	Color color = Color.HSVToRGB(hue / 360, 1f, intensity);

        	colors[x] = color;
    	}

    	gradientTexture.SetPixels(colors);
    	gradientTexture.Apply();

    	return gradientTexture;
	}
}
