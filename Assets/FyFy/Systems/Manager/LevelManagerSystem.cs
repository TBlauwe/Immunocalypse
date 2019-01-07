using UnityEngine;
using UnityEngine.UI;
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

    private bool computeDebriefInformations = false;

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

        manager.nextLevel.onClick.AddListener(nextState);
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
                if (!computeDebriefInformations)
                    computeDebrief();
                break;

            case 2: // Save & Go to next scene
                Global.player.levelDeck.Clear();

                // Reset statistics
                Global.data.trackedEntities.Clear();
                foreach(EStatTrackedEntity trackedEntity in Enum.GetValues(typeof(EStatTrackedEntity)))
                {
                    Global.data.trackedEntities.Add(new PairEStatTrackedEntityInt(trackedEntity, 0));
                }

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
    private void computeDebrief()
    {
        computeDebriefInformations = true;
        int note = 0;

        // DETAILS
        if (manager.won)
        {
            manager.details.text = Global.data.currentLevelWinDescription;
        }
        else
        {
            manager.details.text = Global.data.currentLevelLostDescription;
        }

        // UNLOCKABLES
        foreach (GameObject go in Global.data.currentLevelCardRewards)
        {
            if(go != null)
            {
                GameObject clone = Utility.clone(go, manager.cardsUnlockableScrollView);
                clone.transform.localScale = new Vector3(1, 1, 1);
                clone.transform.localPosition = new Vector3(0, 0, 0);
                clone.transform.localEulerAngles = new Vector3(0, 0, 0);
                clone.GetComponent<Button>().interactable = manager.won;
            }
        }

        foreach (EGalleryModel model in Global.data.currentLevelGalleryModelRewards)
        {
            GameObject go = Utility.clone(manager.Text_ContentSizeFitter_Prefab, manager.galleryModelsUnlockableScrollView);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);
            go.GetComponent<Text>().text = "Modèle " + model.ToString();
            go.GetComponent<Text>().color = (manager.won) ? Color.green : Color.red;
        }

        // ========== STATISTICS ==========
        foreach (PairEStatTrackedEntityInt targetStat in Global.data.targetStats)
        {
            manager.totalNote++;
            // I. First, add all expexted stat from the level
            GameObject go = Utility.clone(manager.Stat_Prefab, manager.statisticsScrollView);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            UI_Stat stat = go.GetComponent<UI_Stat>();
            int target = targetStat.b;
            int actual = 0;

            for(int i = 0; i < Global.data.trackedEntities.Count; i++)
            {
                PairEStatTrackedEntityInt actualStat = Global.data.trackedEntities[i];
                if(actualStat.a == targetStat.a)
                {
                    actual = actualStat.b;
                    Global.data.trackedEntities.RemoveAt(i);
                    break;
                }
            }

            // Set stat's text
            stat.nameText = targetStat.a.ToString();
            stat.targetText = target.ToString();
            stat.actualText = actual.ToString();

            // Set stat's color
            if(actual < target)
            {
                note -= 2;
                stat.Actual.color = Color.blue;
            }else if(actual == target)
            {
                note -= 1;
                stat.Actual.color = Color.green;
            }
            else
            {
                note++;
                stat.Actual.color = Color.red;
            }
        }

        // II. Second, add all unexpected stat 
        for(int i = 0; i < Global.data.trackedEntities.Count; i++)
        {
            GameObject go = Utility.clone(manager.Stat_Prefab, manager.statisticsScrollView);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            PairEStatTrackedEntityInt actualStat = Global.data.trackedEntities[i];
            UI_Stat stat = go.GetComponent<UI_Stat>();

            stat.nameText = actualStat.a.ToString();
            stat.targetText = 0.ToString();
            stat.actualText = actualStat.b.ToString();
            stat.Actual.color = Color.green;

            note += 2;
        }
        manager.note = Mathf.Clamp(note, 0, manager.totalNote);

        manager.noteText.text = manager.note.ToString() + " / " + manager.totalNote.ToString();
    }

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