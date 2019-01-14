using UnityEngine;
using FYFY;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeckManagerSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonManager = FamilyManager.getFamily(new AllOfComponents(typeof(DeckManager)));

    private DeckManager manager;

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public DeckManagerSystem()
    {
        manager = singletonManager.First().GetComponent<DeckManager>();
        if (!manager)
        {
            Debug.LogError("No deck manager in Scene");
            return;
        }
        manager.Description.text = Global.data.currentLevelDescription;
        manager.buttonLoadScene.onClick.AddListener(Fight);

        foreach (GameObject go in Global.player.globalDeck)
        {
            if(go != null)
            {
                GameObject clone = Utility.clone(go, manager.globalDeck);
                clone.transform.localScale = new Vector3(1, 1, 1);
                clone.transform.localPosition = new Vector3(0, 0, 0);
                clone.transform.localEulerAngles = new Vector3(0, 0, 0);
                clone.GetComponent<Button>().onClick.AddListener(delegate { SwitchDeck(clone); });
            }
        }
    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
    private void Fight()
    {
        foreach(Transform child in manager.deck.transform)
        {
            Card card = child.gameObject.GetComponent<Card>();
            for (int i = 0; i < card.counter; i++)
                Global.player.levelDeck.Add(card.entityPrefab);
        }
        if(Global.data.currentInstructionsScene != EInstructions.None)
        {
            GameObjectManager.loadScene(Global.data.currentInstructionsScene.ToString());
        }
    }

    private void SwitchDeck(GameObject go)
    {
        if (go.transform.parent == manager.globalDeck.transform)
            moveToParent(go, manager.deck);
        else
            moveToParent(go, manager.globalDeck);
    }

    private void moveToParent(GameObject go, GameObject parent)
    {
        go.transform.SetParent(parent.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.localPosition = new Vector3(0, 0, 0);
    }
}