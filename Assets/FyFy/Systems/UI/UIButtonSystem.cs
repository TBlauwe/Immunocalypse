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
            case "MenuPrincipal":
                button.onClick.AddListener(delegate { MenuPrincipal(); });
                break;
            case "LoadLevel":
                button.onClick.AddListener(delegate { LoadLevel(go); } );
                break;
            case "ToggleCardMenu":
                button.onClick.AddListener(delegate { ToggleCardMenu(go); } );
                break;
            case "Fight":
                button.onClick.AddListener(delegate { Figth(go); } );
                break;
            default:
                Debug.LogError("GameObject : " + go.name + " | Function name : " + functionName + " | Unknown");
                break;
        }
    }



    // ========================================
    // ========== BUTTON'S FUNCTIONS ==========
    // ========================================
    public void MenuPrincipal()
    {
        /**
        if (SceneManager.GetActiveScene().name == "PierreScene" || SceneManager.GetActiveScene().name == "PrepareDeckScene")
        {
            GameObject _player = GameObject.Find("Player"); // Could be replaced with a Family searching for Player component
            Player player = _player.GetComponent<Player>();

            foreach (GameObject card in _cards)
            {
                // Card are no more active and in-game
                card.transform.SetParent(_player.transform);
                card.SetActive(false);
                card.GetComponent<Card>().isInDeck = false;
            }

            // Everything must return to the global deck
            player.globalDeck.AddRange(player.levelDeck);
        }
        **/
        GameObjectManager.loadScene("MainMenu");
    }

    public void LoadLevel(GameObject go)
    {
        LevelButton comp = go.gameObject.GetComponent<LevelButton>();
        comp.selected = true;
    }

    public void ToggleCardMenu(GameObject go)
    {
        GameObject panel = go.transform.parent.Find("Deck").gameObject;
        panel.SetActive(!panel.activeSelf);

        GameObject button = go.transform.parent.Find("ToggleButton").gameObject;
        button.transform.eulerAngles += Vector3.forward * 180;
    }

    public void Figth(GameObject go)
    {
        GameObjectManager.loadScene(Global.data.currentPlayScene.ToString());
    }

}