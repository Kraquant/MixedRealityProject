using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EraserButton : MonoBehaviour
{
	public GameObject eraserFrame;
	private bool eraserMode;
	
	private void Start()
	{
		eraserMode = false;
	}
	
    public void Toggle()
    {
        eraserFrame.SetActive(!eraserFrame.activeSelf);
        eraserMode = eraserFrame.activeSelf;
    }
    
    public bool IsEraserMode()
    {
    	return eraserMode;    
    }
}
