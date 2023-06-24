using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintModeToggle : MonoBehaviour
{
    public GameObject sprayCan;

    public void Toggle()
    {
        sprayCan.SetActive(!sprayCan.activeSelf);
    }
}
