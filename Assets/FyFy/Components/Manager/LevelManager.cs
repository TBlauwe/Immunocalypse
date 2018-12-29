using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public bool isPlaying = false;      // Le niveau ne commence pas tout de suite :

    public GameObject instructions;     // Etape I      - Affichage des instructions
    public GameObject playing;          // Etape II     - Partie
    public GameObject debrief;          // Etape III    - Débrief

    public Button instructionsToPlaying;
    public Button debriefToNext;

    public int state=0;                    // Index de l'étape en cours
}