using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBoxUI : MonoBehaviour
{
    [SerializeField] private Image colorBoxImage;

    public void UpdateColor(Color color)
    {
        colorBoxImage.color = color;
    }
}
