using UnityEngine;
using FYFY;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeckManagerSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonManager = FamilyManager.getFamily(new AllOfComponents(typeof(DeckManager)));
    private Family _cardGO = FamilyManager.getFamily(new AllOfComponents(typeof(Card)));

    private DeckManager manager;

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public DeckManagerSystem()
    {
        _cardGO.addEntryCallback(InitializeCard);

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
        GameObjectManager.loadScene("InstructionsScene");
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

    /// <summary>
    ///     Use this method for (optionally) initialize your card with the provided text and image.
    /// </summary>
    /// <param name="go">The GameObject having the Card component</param>
    private void InitializeCard(GameObject go)
    {
        Card card = go.GetComponent<Card>();

        TMPro.TextMeshProUGUI Text = go.transform.Find("Title").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        Text.text = card.title;

        TMPro.TextMeshProUGUI Counter = go.transform.Find("Counter").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        Counter.text = card.counter.ToString();
    }

    private List<GameObject> spawnGameObjectFromCard(Card card)
    {
        List<GameObject> spawnedGOs = new List<GameObject>();
        if(card.counter > 0)
        {
            for(int i = 0; i<card.counter; i++)
            {
                // Instanciate prefab
                GameObject clone = Object.Instantiate(card.entityPrefab);
                clone.name = card.title + " - " + i.ToString();
                Dragable drag = clone.AddComponent<Dragable>();
                drag.isDragged = true;

                // Bind it to FYFY
                GameObjectManager.bind(clone);
                spawnedGOs.Add(clone);
            }
        }
        return spawnedGOs;
    }
}