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
        GameObjectManager.loadScene("PrepareDeckScene");
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
            GameObject player = GameObject.Find("Player");
            foreach (GameObject card in _cards)
            {
                foreach (Deck deck in player.GetComponents<Deck>())
                {
                    if (!deck.inGame)
                    {
                        deck.cards.Clear();
                        foreach (GameObject go in _cards)
                        {
                            deck.cards.Add(go);
                            go.transform.SetParent(player.transform);
                            go.SetActive(false);
                        }
                    }
                }
            }
        }
        GameObjectManager.loadScene("MainMenu");
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
        GameObject globalHolder = GameObject.Find("GlobalCardHolder");
        GameObject levelHolder = GameObject.Find("LevelCardHolder");

        foreach (Deck deck in player.GetComponents<Deck>())
        {
            if (deck.inGame)
            {
                deck.cards.Clear();
                foreach (Transform tr in levelHolder.transform)
                {
                    deck.cards.Add(tr.gameObject);
                    tr.gameObject.transform.SetParent(player.transform);
                    tr.gameObject.SetActive(false);
                }
            }
            else
            {
                deck.cards.Clear();
                foreach (Transform tr in globalHolder.transform)
                {
                    deck.cards.Add(tr.gameObject);
                    tr.gameObject.transform.SetParent(player.transform);
                    tr.gameObject.SetActive(false);
                }
            }
        }

        GameObjectManager.loadScene("PierreScene");
    }
}