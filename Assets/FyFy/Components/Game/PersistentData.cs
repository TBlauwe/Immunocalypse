using System;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour {
    // ========== PLAYER ==========
    public List<PairELevelBool> succeededLevels = new List<PairELevelBool>();
    public List<PairEGalleryModelBool> unlockedGalleryModels = new List<PairEGalleryModelBool>(); // Unlock those models in the gallery

    // ========== LEVEL ==========
    public ELevel currentLevel;
    public EGalleryModel currentLevelGalleryModelReward;
    public string currentLevelDescription;

    // ========== STATISTICS ==========
    // - Reset for each new level
    // - Used to debrief a player after finishing a level
}