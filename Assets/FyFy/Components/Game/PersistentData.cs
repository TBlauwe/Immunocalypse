using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour {

    // ========== LEVEL ==========
    public string selectedLevelId;
    public string selectedDifficultyId;
    public string selectedDifficultyDescription;
    public string selectedDifficultyScene;

    // ========== STATISTICS ==========
    // - Reset for each new level
    // - Used to debrief a player after finishing a level
}