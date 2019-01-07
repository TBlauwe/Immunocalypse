using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public string nameText;
    public string actualText;
    public string targetText;

    public Text Name;
    public Text Actual;
    public Text Target;
}