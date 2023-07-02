using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamingToggle : MonoBehaviour
{
	public GameObject streamingMenu;
	
    public void Toggle()
    {
        streamingMenu.SetActive(!streamingMenu.activeSelf);        
    }
}
