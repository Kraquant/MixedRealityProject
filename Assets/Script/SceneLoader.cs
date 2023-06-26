using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.3f;
    public List<string> sceneName;
    public List<Button> buttonList;

    private void Start()
    {
        foreach (Button button in buttonList)
        {
            button.onClick.AddListener(
                () => ButtonClicked(buttonList.IndexOf(button)));
        }
    }

    private void ButtonClicked(int buttonNo)
    {
        LoadScene(sceneName[buttonNo]);
    }
    private void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene");
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        // Play Animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load Scene
            SceneManager.LoadScene(sceneName);
    }
    public void QuitLevel()
    {
        PlayerPrefs.SetInt("LoadSaved", 1);
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Quit Level");
    }
}
