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
        bool showGeneralDescription = true;

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
                showGeneralDescription = false;

                manager.levelTitleText.text = levelButton.title;
                manager.levelDescriptionText.text = levelButton.description;
            }
        }
        manager.generalDescriptionPanel.SetActive(showGeneralDescription);
        manager.levelDescriptionPanel.SetActive(!showGeneralDescription);
        manager.buttonNextMenu.GetComponent<Button>().interactable = !showGeneralDescription;
    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
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
        DifficultyLevel comp = go.gameObject.GetComponent<DifficultyLevel>();
        Global.data.currentPlayScene = comp.playScene;
        Global.data.currentInstructionsScene = comp.instructionsScene;
        Global.data.currentLevelDescription = comp.description;
        Global.data.currentLevelWinDescription = comp.wonDescription;
        Global.data.currentLevelLostDescription = comp.lostDescription;
        Global.data.currentLevelGalleryModelRewards = comp.galleryModelRewards;
        Global.data.currentLevelCardRewards = comp.cardRewards;
        Global.data.targetStats = comp.goodStats;

        GameObjectManager.loadScene("DeckBuilderScene");
    }
}