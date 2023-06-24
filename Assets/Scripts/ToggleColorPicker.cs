using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorPicker : MonoBehaviour
{
    public GameObject colorPickerContainer;

    public void Toggle()
    {
        colorPickerContainer.SetActive(!colorPickerContainer.activeSelf);
    }
}
