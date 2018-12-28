using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class LevelSelectorSystem : FSystem {

    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family          singletonLevelSelectionManager  = FamilyManager.getFamily(new AllOfComponents(typeof(LevelSelectionManager)));
    private Family          _levelGO                        = FamilyManager.getFamily(new AllOfComponents(typeof(LevelButton)));
    private Family          _DifficultyGO                   = FamilyManager.getFamily(new AllOfComponents(typeof(DifficultyLevel)));

    private LevelSelectionManager   manager;
    private LevelButton             selectedLevel;
    private DifficultyLevel         selectedDifficulty;

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public LevelSelectorSystem()
    {
        // ===== Difficulty GOs setup =====
        foreach(GameObject go in _DifficultyGO)
        {
            DifficultyLevel  difficultyLevel = go.gameObject.GetComponent<DifficultyLevel>();
            difficultyLevel.Title.text = difficultyLevel.title;
            difficultyLevel.Descrition.text = difficultyLevel.description;
            go.GetComponent<Button>().onClick.AddListener(delegate { loadDeckBuilderScene(go); });
        }

        // ===== Manager setup =====
        manager = singletonLevelSelectionManager.First().GetComponent<LevelSelectionManager>();
        manager.buttonNextMenu.onClick.AddListener(delegate { toggleDifficultyLevels(); });
        manager.buttonPrevMenu.onClick.AddListener(delegate { toggleDifficultyLevels(); });
    }

    // ===========================
    // ========== LOOPS ==========
    // ===========================
    protected override void onProcess(int familiesUpdateCount)
    {
        foreach(GameObject go in _levelGO)
        {
            LevelButton levelButton = go.gameObject.GetComponent<LevelButton>();
            if (levelButton.selected)
            {
                if (selectedLevel != levelButton && selectedLevel != null)
                {
                    selectedLevel.selected = false;
                }
                selectedLevel = levelButton;

                foreach(Transform child in levelButton.informationPanel.transform)
                {
                    if(child.gameObject.name == "Title")
                    {
                        TMPro.TextMeshProUGUI Title = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                        Title.text = levelButton.title;
                    }
                    else if(child.gameObject.name == "Description")
                    {
                        TMPro.TextMeshProUGUI Description = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                        Description.text = levelButton.description;
                    }
                }
            }
        }

        foreach(GameObject go in _DifficultyGO)
        {
            DifficultyLevel  difficultyLevel = go.gameObject.GetComponent<DifficultyLevel>();
            if (difficultyLevel.selected)
            {
                if (selectedDifficulty != difficultyLevel && selectedDifficulty != null)
                {
                    selectedDifficulty.selected = false;
                }
                selectedDifficulty = difficultyLevel;
            }
        }
    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
    private void setupDifficultyLevel(GameObject go)
    {
        DifficultyLevel difficultyLevel = go.GetComponent<DifficultyLevel>();
        difficultyLevel.Title.text = difficultyLevel.title;
        difficultyLevel.Descrition.text = difficultyLevel.description;
    }

    public void toggleDifficultyLevels()
    {
        if (selectedLevel)
        {
            manager.isOnMenuDifficultySelection = !manager.isOnMenuDifficultySelection;
            manager.menuLevelSelection.SetActive(!manager.isOnMenuDifficultySelection);
            manager.menuDifficultySelection.SetActive(manager.isOnMenuDifficultySelection);
            selectedLevel.difficultyLevels.SetActive(manager.isOnMenuDifficultySelection);
        }
    }

    public void loadDeckBuilderScene(GameObject go)
    {
        if (selectedLevel)
        {
            DifficultyLevel comp = go.gameObject.GetComponent<DifficultyLevel>();
            comp.selected = true;
            Global.data.selectedDifficultyId = comp.title;
            Global.data.selectedLevelDescription = comp.description;
            Global.data.factories = comp.factories;

            GameObjectManager.loadScene("4_DeckBuilderScene");
        }
    }
}