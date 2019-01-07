using UnityEngine;
using FYFY;
using System.Collections.Generic;
using System;

public class LevelManagerSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private readonly Family singletonManager = FamilyManager.getFamily(new AllOfComponents(typeof(LevelManager)));

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

        manager.victoryFinishLevel.onClick.AddListener(nextState);
        manager.defeatFinishLevel.onClick.AddListener(nextState);
        refreshState();
        cachedState = manager.state;

        // Reset statistics
        Global.data.trackedEntities.Clear();
        foreach(EStatTrackedEntity trackedEntity in Enum.GetValues(typeof(EStatTrackedEntity)))
        {
            Global.data.trackedEntities.Add(new PairEStatTrackedEntityInt(trackedEntity, 0));
        }

        // Setup pool
        spawner = manager.bloodVessel.GetComponent<StartLoopTrigger>();
        spawner.deckPool = Global.player.levelDeck;

        // Setup "unlockable" description
        foreach (GameObject go in Global.data.currentLevelCardRewards)
        {
            if(go != null)
            {
                GameObject clone = Utility.clone(go, manager.cardsUnlockablePanel);
                clone.transform.localScale = new Vector3(1, 1, 1);
                clone.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        foreach (EGalleryModel model in Global.data.currentLevelGalleryModelRewards)
        {
            manager.galleryModelsUnlockableText.text += "Le modèle " + model.ToString() + "\n";
        }
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
        manager.wonPanel.SetActive(manager.state == 1 && manager.won);
        manager.lostPanel.SetActive(manager.state == 1 && !manager.won);
    }

    private void endPlay()
    {
        if (manager.won)
        {
            // Level is won
            foreach(PairELevelBool level in Global.data.succeededLevels)
            {
                if(level.a == Global.data.currentPlayScene)
                {
                    level.b = true;
                }
            }

            foreach (EGalleryModel model in Global.data.currentLevelGalleryModelRewards)
            {
                foreach(PairEGalleryModelBool galleryModel in Global.data.unlockedGalleryModels)
                {
                    if (galleryModel.a == model)
                    {
                        galleryModel.b = true;
                        break;
                    }
                }
            }

            foreach (GameObject reward in Global.data.currentLevelCardRewards)
            {
                Global.player.globalDeck.Add(reward);
            }
        }

        GameObjectManager.loadScene("MainMenu");
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
                    infectedFound = true;
            }

            if (!infectedFound && RemainingFactories.Count == 0 && RemainingPathogenes.Count == 0)
            {
                manager.won = true;
                nextState();
            }
        }
    }
}