using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    // ========== GAME ==========
    public bool won = false;            // Est-ce que le joueur a gagné ?

    public GameObject playing;          // Etape II     - Partie
    public GameObject debrief;          // Etape III    - Débrief
    public GameObject bloodVessel;      // 

    // ========== DEBRIEF ==========
    public Text       details;         // 

    public GameObject cardsUnlockableScrollView;       
    public GameObject galleryModelsUnlockableScrollView;       
    public GameObject statisticsScrollView;       

    public GameObject Text_ContentSizeFitter_Prefab;       
    public GameObject Stat_Prefab;       

    public Text note;

    // ========== UI ==========
    public Button nextLevel;

    public int state=0;                    // Index de l'étape en cours
}