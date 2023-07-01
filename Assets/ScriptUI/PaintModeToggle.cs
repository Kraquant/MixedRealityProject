using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintModeToggle : MonoBehaviour
{
    public GameObject sprayCan;
    private bool sprayMode; //true for spray. false for finger
    
    public Image selfImage;
    public Image paintButtonImage;
    public GameObject pointerImage; //GameObject to access SetActive

    public Sprite sprayCanSprite;
    public Sprite fingerSprite;

    private BasicRaycast touchRaycast;
    private ButtonRaycast sprayRaycast;
    
    private void Start()
    {
        touchRaycast = sprayCan.GetComponent<BasicRaycast>();
        sprayRaycast = sprayCan.GetComponent<ButtonRaycast>();
        touchRaycast.modeEnabled = false;
        sprayRaycast.modeEnabled = true;
    	sprayMode = true;
    }
    
    public bool GetSprayMode()
    {
    	return sprayMode;
    }

    public void Toggle()
    {
    	//visuals
    	sprayMode = !sprayMode;
    	selfImage.sprite = sprayMode ? fingerSprite : sprayCanSprite;
    	paintButtonImage.sprite = sprayMode ? sprayCanSprite : fingerSprite;
    	pointerImage.SetActive(sprayMode); //pointer only visible in spray mode.
    	
    	//raycasting
        touchRaycast.modeEnabled = !sprayMode;
        sprayRaycast.modeEnabled = sprayMode;
        
        //sprayCan.SetActive(!sprayCan.activeSelf);
        //sprayMode = sprayCan.activeSelf;
    }
}
