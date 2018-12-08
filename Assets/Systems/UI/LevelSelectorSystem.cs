using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class LevelSelectorSystem : FSystem {

    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family _levelGO = FamilyManager.getFamily(new AllOfComponents(typeof(LevelButton)));

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public LevelSelectorSystem()
    {
        foreach(GameObject go in _levelGO)
        {
            setupLevelButton(go);
        }

        _levelGO.addEntryCallback(DontDestroyCallback);
    }

    // ===========================
    // ========== LOOPS ==========
    // ===========================
    protected override void onProcess(int familiesUpdateCount)
    {
        foreach(GameObject go in _levelGO)
        {
            refresh(go);
        }
    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
    private void setupLevelButton(GameObject go)
    {
        LevelButton levelButton = go.GetComponent<LevelButton>();
        
        // Setup UI 
        setLevelName(go, levelButton.levelName);

        // Setup cache variables
        levelButton.victoryCached = levelButton.victory;

        // Store initial color values 
        ColorBlock colors = go.GetComponent<Button>().colors;
        levelButton.normalColor = colors.normalColor;
        levelButton.highlightedColor = colors.highlightedColor;
    }

    private void refresh(GameObject go)
    {
        togglePanel(go);
        if (hasStateChanged(go))
        {
            refreshColor(go);
        }
    }

    private bool areParentsWon(LevelButton levelButton)
    {
        foreach (GameObject go in levelButton.parents)
        {
            LevelButton comp = go.GetComponent<LevelButton>();
            if (!comp.victory) { return false; }
        }
        return true;
    }

    private bool hasStateChanged(GameObject go)
    {
        LevelButton levelButton = go.GetComponent<LevelButton>();
        if(levelButton.victory != levelButton.victoryCached)
        {
            levelButton.victoryCached = levelButton.victory;
            return true;
        }
        return false;
    }

    private void refreshColor(GameObject go)
    {
        LevelButton levelButton = go.GetComponent<LevelButton>();
        ColorBlock colors       = go.GetComponent<Button>().colors;
        if (levelButton.victory)
        {
            colors.normalColor = levelButton.victoryNormalColor;
            colors.highlightedColor = levelButton.victoryHighlightedColor;
        }
        else
        {
            colors.normalColor = levelButton.normalColor;
            colors.highlightedColor = levelButton.highlightedColor;
        }
        go.GetComponent<Button>().colors = colors;
    }

    private void togglePanel(GameObject go)
    {
        LevelButton levelButton = go.GetComponent<LevelButton>();

        if (areParentsWon(levelButton)){
            go.transform.Find("Unlock").gameObject.SetActive(true);
            go.transform.Find("Lock").gameObject.SetActive(false);
            go.GetComponent<Button>().interactable = true;
        }
        else {
            go.transform.Find("Unlock").gameObject.SetActive(false);
            go.transform.Find("Lock").gameObject.SetActive(true);
            go.GetComponent<Button>().interactable = false;
        }
    }
    
    // A call back function telling that all player objects shouldn't be destroyed
    private void DontDestroyCallback(GameObject player)
    {
        GameObjectManager.dontDestroyOnLoadAndRebind(player);
    }

    // =======================================
    // ========== GETTERS & SETTERS ==========
    // =======================================

    private void setLevelName(GameObject go, string value)
    {
        Text text = (Text) go.transform.Find("Unlock/name").gameObject.GetComponent<Text>();
        text.text = value;
    }
}