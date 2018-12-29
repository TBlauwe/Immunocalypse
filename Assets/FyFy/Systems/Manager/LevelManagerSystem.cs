using UnityEngine;
using FYFY;
using System.Collections.Generic;

public class LevelManagerSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonManager = FamilyManager.getFamily(new AllOfComponents(typeof(LevelManager)));
    private LevelManager manager;
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

        manager.instructionsToPlaying.onClick.AddListener(nextState);
        manager.debriefToNext.onClick.AddListener(nextState);
        refreshState();
        cachedState = manager.state;
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
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
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
        manager.instructions.SetActive(manager.state == 0);
        manager.playing.SetActive(manager.state == 1);
        manager.debrief.SetActive(manager.state == 2);
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

        GameObjectManager.loadScene("LevelSelectionScene");
    }
}