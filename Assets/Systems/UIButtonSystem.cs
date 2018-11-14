using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FYFY;

public class UIButtonSystem : FSystem
{

    // =============================
    // ========== MEMBERS ==========
    // =============================

    private Family _UIClickableButtons = FamilyManager.getFamily(new AllOfComponents(typeof(UI_Button)));

    // =================================
    // ========== CONSTRUCTOR ==========
    // =================================

    public UIButtonSystem()
    {
        foreach (GameObject go in _UIClickableButtons)
        {
            UI_Button onClickFunctions = go.GetComponent<UI_Button>();
            Button button = go.GetComponent<Button>();

            foreach (string functionName in onClickFunctions.functionNames)
            {
                Debug.Log("adding " + functionName);
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
            case "MenuPrincipal":
                button.onClick.AddListener(MenuPrincipal);
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

    public void MenuPrincipal()
    {
        Debug.Log("clicked");
        SceneManager.LoadScene("MainMenu");
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