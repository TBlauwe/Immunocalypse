using System;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour {
    // ========== PLAYER ==========
    public List<PairELevelBool> succeededLevels = new List<PairELevelBool>();
    public List<PairEGalleryModelBool> unlockedGalleryModels = new List<PairEGalleryModelBool>(); // Unlock those models in the gallery

    // ========== LEVEL ==========
    public ELevel currentPlayScene;
    public EInstructions currentInstructionsScene;
    public List<EGalleryModel> currentLevelGalleryModelRewards;
    public List<GameObject> currentLevelCardRewards;
    public string currentLevelDescription;
    public string currentLevelLostDescription;
    public string currentLevelWinDescription;

    // ========== STATISTICS ==========
    // - Reset for each new level
    // - Used to debrief a player after finishing a level
    public List<PairEStatTrackedEntityInt> targetStats = new List<PairEStatTrackedEntityInt>(); // Unlock those models in the gallery
    public List<PairEStatTrackedEntityInt> trackedEntities = new List<PairEStatTrackedEntityInt>(); // Unlock those models in the gallery
}