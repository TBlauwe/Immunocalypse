using UnityEngine;
using FYFY;
using System.Collections.Generic;

public class LevelManagerSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonManager = FamilyManager.getFamily(new AllOfComponents(typeof(LevelManager)));
    private readonly Family RemainingCells = FamilyManager.getFamily(
        new AllOfComponents(typeof(Cell)),
        new NoneOfComponents(typeof(Removed))
    );
    private readonly Family RemainingPathogenes = FamilyManager.getFamily(
        new AnyOfLayers(9) // pathogene layer
    );
    private readonly Family RemainingFactories = FamilyManager.getFamily(
        new AllOfComponents(typeof(Factory))
    );

    private LevelManager manager;
    private StartLoopTrigger spawner;
    private int cachedState;

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public LevelManagerSystem()
    {
        manager = singletonManager.First().GetComponent<LevelManager>();

        if (!manager)
        {
            Debug.LogError("No manager in Scene");
            return;
        }

        manager.finishLevel.onClick.AddListener(nextState);
        refreshState();
        cachedState = manager.state;

        // Setup pool
        spawner = manager.bloodVessel.GetComponent<StartLoopTrigger>();
        spawner.deckPool = Global.player.levelDeck;
    }

    protected override void onProcess(int familiesUpdateCount)
    {
        if (cachedState != manager.state)
        {
            refreshState();
            cachedState = manager.state;
        }

        switch (manager.state)
        {
            case 0: // Playing
                UpdatePlayerStatus();
                break;
            case 1: // Debrief
                break;
            case 2: // Save & Go to next scene
                Global.player.levelDeck.Clear();
                endPlay();
                break;
            default:
                Debug.LogError("Unknown GameState");
                break;
        }
    }


    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
    private void nextState()
    {
        manager.state++;
        cachedState = manager.state;
        refreshState();
    }

    private void refreshState()
    {
        manager.playing.SetActive(manager.state == 0);
        manager.debrief.SetActive(manager.state == 1);
    }

    private void endPlay()
    {
        if (manager.won)
        {
            // Level is won
            foreach(PairELevelBool level in Global.data.succeededLevels)
            {
                if(level.a == Global.data.currentLevel)
                {
                    level.b = true;
                }
            }

            foreach(PairEGalleryModelBool galleryModel in Global.data.unlockedGalleryModels)
            {
                if(galleryModel.a == Global.data.currentLevelGalleryModelReward)
                {
                    galleryModel.b = true;
                }
            }

        }

        GameObjectManager.loadScene("MenuPrincipalScene");
    }

    private void UpdatePlayerStatus()
    {
        if (RemainingCells.Count == 0)
        {
            manager.won = false;
            nextState();
        }
        else
        {
            bool infectedFound = false;
            foreach (GameObject cellGO in RemainingCells)
            {
                Cell cell = cellGO.GetComponent<Cell>();
                if (!cell.state.Equals(CellState.HEALTHY))
                {
                    infectedFound = true;
                }
            }
            if (!infectedFound && RemainingFactories.Count == 0 && RemainingPathogenes.Count == 0)
            {
                manager.won = true;
                nextState();
            }
        }
    }
}