using UnityEngine;
using FYFY;

public class DeckManagerSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonDeckManager = FamilyManager.getFamily(new AllOfComponents(typeof(DeckManager)));

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public DeckManagerSystem()
    {
        DeckManager deckManager = singletonDeckManager.First().GetComponent<DeckManager>();

        if (!deckManager)
        {
            Debug.LogError("No deck manager in Scene");
            return;
        }
        deckManager.Description.text = Global.data.selectedLevelDescription;
    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
}