using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EraserButton : MonoBehaviour
{
	public GameObject eraserFrame;
	
	private void Start()
	{
		Painter.eraser = false;
	}
	
    public void Toggle()
    {
        eraserFrame.SetActive(!eraserFrame.activeSelf);
        Painter.eraser = eraserFrame.activeSelf;
    }
    
    public bool IsEraserMode()
    {
    	return Painter.eraser;    
    }
}
