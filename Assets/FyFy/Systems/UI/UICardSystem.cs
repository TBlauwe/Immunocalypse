using UnityEngine;
using FYFY;
using UnityEngine.UI;
using FYFY_plugins.PointerManager;
using System.Collections.Generic;
using FYFY_plugins.TriggerManager;

/// <summary>
///     This System is designed to manage cards in the User Interface.
/// </summary>
public class UICardSystem : FSystem {

    // Where cards can be displayed
    private readonly Family _holders = FamilyManager.getFamily(new AllOfComponents(typeof(CardHolder)));

    // All the available cards in the current scene (and active)
    private readonly Family _cards = FamilyManager.getFamily(
        new AllOfComponents(typeof(Card)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );

    // All active cards with the mouse point over
    private readonly Family _hoveredCards = FamilyManager.getFamily(
        new AllOfComponents(typeof(Card), typeof(PointerOver)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );

    private readonly GameObject UI = GameObject.Find("DeckBuilder");

    public UICardSystem()
    {
        // Each time a new card is added to the family, we must initialize it
        _cards.addEntryCallback(InitializeCard);

        // Initialize already present cards if necessary
        foreach (GameObject go in _cards)
        {
            InitializeCard(go);
        }

        // Manage card holders
        foreach (GameObject go in _holders)
        {
            CardHolder holder = go.GetComponent<CardHolder>();
            if (holder.inGame) // Holder in a play scene
            {
                MoveCardsToHolder(go, Global.player.levelDeck, true);
            }

            else if (go.name == "GlobalCardHolder") // Holder in preparedeck scene
            {
                MoveCardsToHolder(go, Global.player.globalDeck, false);
            }
        }

    }

    /// <summary>
    ///     Process families.
    /// </summary>
    /// <param name="familiesUpdateCount"></param>
    protected override void onProcess(int familiesUpdateCount) {

        // Manage hovered cards
        foreach (GameObject go in _hoveredCards)
        {
            handleDragAndDrop(go);
        }
	}

    /// <summary>
    ///     Use this method for (optionally) initialize your card with the provided text and image.
    /// </summary>
    /// <param name="go">The GameObject having the Card component</param>
    private void InitializeCard(GameObject go)
    {
        Card card = go.GetComponent<Card>();
        // Initialize card if not done yet
        if (!card.initialized)
        {
            TMPro.TextMeshProUGUI Text = go.transform.Find("Title").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            Text.text = card.title;

            TMPro.TextMeshProUGUI Counter = go.transform.Find("Counter").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            Counter.text = card.counter.ToString();
            card.initialized = true;
        }
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

    private void handleDragAndDrop(GameObject go)
    {
        Dragable dragable = go.GetComponent<Dragable>();

        if (Input.GetMouseButtonDown(0) && !dragable.isDragged)
        {            
            Vector2 mousePos = new Vector2
            {
                // Get the mouse position from Input.
                // Note that the y position from Input is inverted.
                x = Input.mousePosition.x,
                y = Input.mousePosition.y
            };

            // Tell the card it is moving
            dragable.isDragged = true;

            // Remove card from parent holder
            go.transform.SetParent(UI.transform);
            go.transform.position = mousePos;
        }
    }

    private void MoveCardsToHolder(GameObject holder, List<GameObject> cards, bool isInDeck)
    {
        foreach (GameObject card in cards)
        {
            // Activate card and set it as an in-game card if necessary
            card.SetActive(true);

            // Add it to the holder
            card.transform.SetParent(holder.transform);
            card.transform.localScale = new Vector3(1, 1, 1);
            card.transform.SetAsFirstSibling();
        }
    }

    private void MoveCardToGlobalDeck(GameObject card)
    {
        // Change card parent
        card.transform.SetParent(Global.player.gameObject.transform);

        // Change card's deck
        Global.player.globalDeck.Add(card);
        Global.player.levelDeck.Remove(card);
    }
}