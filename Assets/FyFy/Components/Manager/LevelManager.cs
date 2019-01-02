using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public bool won = false;            // Est-ce que le joueur a gagné ?

    public GameObject playing;          // Etape II     - Partie
    public GameObject debrief;          // Etape III    - Débrief

    public GameObject wonPanel;         // 
    public GameObject lostPanel;        // 

    public GameObject cardsUnlockablePanel;       
    public Text galleryModelsUnlockableText;       

    public GameObject bloodVessel;      // 

    public Button defeatFinishLevel;
    public Button victoryFinishLevel;

    public int state=0;                    // Index de l'étape en cours
}