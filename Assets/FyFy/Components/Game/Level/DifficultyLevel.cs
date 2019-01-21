using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyLevel : MonoBehaviour {
    public string               title;                  // Explicit 
    [TextArea(3,10)]
    public string               description;
    public ELevel               playScene;              // ID - Which scene should be loaded
    public EInstructions        instructionsScene;      // ID - Which scene should be loaded

    public List<GameObject>     randomGOs;              // Pool of random GO that will be available during the level

    public List<EGalleryModel>  galleryModelRewards;    // Unlock this model in the gallery
    public List<GameObject>     cardRewards;            // Add theses cards in the player's global deck

    // Used for debrief to estimate player's performance
    [TextArea(3,10)]
    public string               wonDescription;
    [TextArea(3,10)]
    public string               lostDescription;
    public List<PairEStatTrackedEntityInt>  goodStats = new List<PairEStatTrackedEntityInt>();  

    public Text             Title;                  // UI STUFF
}