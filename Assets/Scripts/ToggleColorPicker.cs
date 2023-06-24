using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorPicker : MonoBehaviour
{
    public GameObject colorPickerContainer;
    public GameObject sprayCan;
    public GameObject paintModeToggle;

    public void Toggle()
    {
        colorPickerContainer.SetActive(!colorPickerContainer.activeSelf);
        //color picker toggles first, and spray can toggles to opposite of color picker. they can never overlap
        sprayCan.SetActive(!colorPickerContainer.activeSelf);
        paintModeToggle.SetActive(!colorPickerContainer.activeSelf);
    }
}
