using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FYFY;

public class UIButtonSystem : FSystem
{

    // =============================
    // ========== MEMBERS ==========
    // =============================

    private Family _UIClickableButtons = FamilyManager.getFamily(new AllOfComponents(typeof(ButtonOnClick)));

    // =================================
    // ========== CONSTRUCTOR ==========
    // =================================

    public UIButtonSystem()
    {
        foreach (GameObject go in _UIClickableButtons)
        {
            ButtonOnClick onClickFunctions = go.GetComponent<ButtonOnClick>();
            Button button = go.GetComponent<Button>();

            foreach (string functionName in onClickFunctions.functionNames)
            {
                addListener(button, functionName);
            }
        }
    }

    // ========================================
    // ========== HELPER'S FUNCTIONS ==========
    // ========================================

    // Add to the switch the function's name you want to be callable from a button
    public void addListener(Button button, string functionName)
    {
        switch (functionName)
        {
            case "Play":
                button.onClick.AddListener(Play);
                break;
            case "FreePlay":
                button.onClick.AddListener(FreePlay);
                break;
            case "Gallery":
                button.onClick.AddListener(Gallery);
                break;
            case "Quit":
                button.onClick.AddListener(Quit);
                break;
            default:
                Debug.LogError("Function name : " + functionName + " | Unknown");
                break;
        }
    }



    // ========================================
    // ========== BUTTON'S FUNCTIONS ==========
    // ========================================
    public void Play()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void FreePlay()
    {
        SceneManager.LoadScene("FreePlayScene");
    }

    public void Gallery()
    {
        SceneManager.LoadScene("GalleryScene");
    }

    public void Quit()
    {
        // save any game data here
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
             Application.Quit();
        #endif
    }
}