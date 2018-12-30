using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public bool won = false;            // Est-ce que le joueur a gagné ?
    public bool isPlaying = false;      // Le niveau ne commence pas tout de suite :

    public GameObject playing;          // Etape II     - Partie
    public GameObject debrief;          // Etape III    - Débrief

    public Button finishLevel;

    public int state=0;                    // Index de l'étape en cours
}