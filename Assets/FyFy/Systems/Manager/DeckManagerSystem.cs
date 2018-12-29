using UnityEngine;
using FYFY;

public class DeckManagerSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonManager = FamilyManager.getFamily(new AllOfComponents(typeof(DeckManager)));

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public DeckManagerSystem()
    {
        DeckManager deckManager = singletonManager.First().GetComponent<DeckManager>();

        if (!deckManager)
        {
            Debug.LogError("No deck manager in Scene");
            return;
        }
        deckManager.Description.text = Global.data.currentLevelDescription;
        deckManager.buttonLoadScene.onClick.AddListener(delegate { Fight(); });
    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
    private void Fight()
    {
        GameObjectManager.loadScene(Global.data.currentLevel.ToString());
    }
}