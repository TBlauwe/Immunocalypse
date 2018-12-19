using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FYFY;
using FYFY_plugins.TriggerManager;

public class UIButtonSystem : FSystem
{
    // =============================
    // ========== MEMBERS ==========
    // =============================

    private Family _UIClickableButtons = FamilyManager.getFamily(new AllOfComponents(typeof(UI_Button)));
    private readonly Family _cards = FamilyManager.getFamily(new AllOfComponents(typeof(Card)));

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
                addListener(go, button, functionName);
            }
        }
    }

    // ========================================
    // ========== HELPER'S FUNCTIONS ==========
    // ========================================

    // Add to the switch the function's name you want to be callable from a button
    public void addListener(GameObject go, Button button, string functionName)
    {
        switch (functionName)
        {
            case "FreePlay":
                button.onClick.AddListener(delegate { FreePlay(); });
                break;
            case "Gallery":
                button.onClick.AddListener(delegate { Gallery(); });
                break;
            case "MenuPrincipal":
                button.onClick.AddListener(delegate { MenuPrincipal(); });
                break;
            case "Play":
                button.onClick.AddListener(delegate { Play(); } );
                break;
            case "LoadLevel":
                button.onClick.AddListener(delegate { LoadLevel(go); } );
                break;
            case "Quit":
                button.onClick.AddListener(delegate { Quit(); });
                break;
            case "ToggleCardMenu":
                button.onClick.AddListener(delegate { ToggleCardMenu(go); } );
                break;
            case "Fight":
                button.onClick.AddListener(delegate { Fight(); });
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
        //SceneManager.LoadScene("PlayScene");
        GameObjectManager.loadScene("LevelSelectorScene");
    }

    public void FreePlay()
    {
        //SceneManager.LoadScene("FreePlayScene");
        GameObjectManager.loadScene("FreePlayScene");
    }

    public void Gallery()
    {
        //SceneManager.LoadScene("GalleryScene");
        GameObjectManager.loadScene("GalleryScene");
    }

    public void MenuPrincipal()
    {
        //SceneManager.LoadScene("MainMenu");
        if (SceneManager.GetActiveScene().name == "PierreScene" || SceneManager.GetActiveScene().name == "PrepareDeckScene")
        {
            GameObject _player = GameObject.Find("Player"); // Could be replaced with a Family searching for Player component
            Player player = _player.GetComponent<Player>();

            foreach (GameObject card in _cards)
            {
                // Card are no more active and in-game
                card.transform.SetParent(_player.transform);
                card.SetActive(false);
                card.GetComponent<Card>().inGame = false;
            }

            // Everything must return to the global deck
            player.globalDeck.AddRange(player.levelDeck);
        }
        GameObjectManager.loadScene("MainMenu");
    }

    public void LoadLevel(GameObject go)
    {
        //SceneManager.LoadScene("GalleryScene");
        LevelButton comp = go.GetComponent<LevelButton>();

        // Save data to be tranfer between scenes
        GameObject _player = GameObject.Find("Player"); // Could be replaced with a Family searching for Player component
        Player player = _player.GetComponent<Player>();
        Debug.Log(player);
        Debug.Log(comp);
        player.levelButton = comp;

        // Load scene and setup scene
        GameObjectManager.loadScene("PrepareDeckScene");
        GameObject descriptionGo  = GameObject.Find("Level Description"); // Could be replaced with a Family searching for Player component
        descriptionGo.GetComponent<TextMesh>().text = comp.description;
    }

    public void ToggleCardMenu(GameObject go)
    {
        GameObject panel = go.transform.parent.Find("Deck").gameObject;
        panel.SetActive(!panel.activeSelf);

        GameObject button = go.transform.parent.Find("ToggleButton").gameObject;
        button.transform.eulerAngles += Vector3.forward * 180;
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

    public void Fight()
    {
        GameObject player = GameObject.Find("Player");

        // Each card returns to the player
        foreach(GameObject card in _cards)
        {
            card.transform.SetParent(player.transform);
        }

        GameObjectManager.loadScene("PierreScene");
    }
}