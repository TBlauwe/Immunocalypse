using UnityEngine;
using FYFY;
using UnityEngine.UI;
using FYFY_plugins.PointerManager;
using System.Collections.Generic;

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
    private readonly Family _hoveredCard = FamilyManager.getFamily(
        new AllOfComponents(typeof(Card), typeof(PointerOver)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );

    // Get all decks
    private readonly Family _decks = FamilyManager.getFamily(
        new AllOfComponents(typeof(Deck))
    );

    public UICardSystem()
    {
        // Each time a new card is added to the family, we must initialize it
        _cards.addEntryCallback(InitializeCard);

        // Initialize already present cards if necessary
        foreach (GameObject go in _cards)
        {
            InitializeCard(go);
        }
    }

    /// <summary>
    ///     Process families.
    /// </summary>
    /// <param name="familiesUpdateCount"></param>
    protected override void onProcess(int familiesUpdateCount) {

        // Manage cards
        foreach (GameObject go in _hoveredCard)
        {
            // Get the card component
            Card card = go.GetComponent<Card>();

            // Switch according to what type of card it is
            if (card.inGame)
            {
                InGameDragAndDrop(go);
            }
            else
            {
                BeforeGameDragAndDrop(go);
            }
            
        }

        // Manage card holders
        foreach (GameObject go in _holders)
        {
            CardHolder holder = go.GetComponent<CardHolder>();
            if (holder.inGame)
            {
                GameObject entity = _decks.First();
                foreach (Deck deck in entity.GetComponents<Deck>())
                {
                    if (deck.inGame)
                    {
                        MoveCardsToHolder(go, deck.cards);
                        deck.cards.Clear();
                    }
                }
            }
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
            Text text = go.transform.Find("Title").gameObject.GetComponent<Text>();
            text.text = card.title;

            Image image = go.transform.Find("Image").gameObject.GetComponent<Image>();
            image.sprite = card.image;

            card.initialized = true;
        }
    }

    private void InGameDragAndDrop(GameObject go)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Card card = go.GetComponent<Card>();
            Vector2 mousePos = new Vector2();
            Vector3 point = new Vector3();

            // Get the mouse position from Input.
            // Note that the y position from Input is inverted.
            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;

            point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
            point.y = 0;

            // Instanciate prefab
            GameObject clone = Object.Instantiate(card.entityPrefab);
            Dragable drag = clone.AddComponent<Dragable>();
            drag.isDragged = true;

            // Bind it to FYFY
            GameObjectManager.bind(clone);

            // Set postion to mouse position
            clone.transform.position = point;

            // De-activate card and remove in-game flag
            MoveCardToGlobalDeck(go);
            go.SetActive(false);
            card.inGame = false;
        }
    }

    private void BeforeGameDragAndDrop(GameObject go)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Card card = go.GetComponent<Card>();
            Vector2 mousePos = new Vector2();
            Vector3 point = new Vector3();

            // Get the mouse position from Input.
            // Note that the y position from Input is inverted.
            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;

            point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
            point.y = 0;

            // Instanciate prefab
            GameObject clone = Object.Instantiate(card.entityPrefab);
            Dragable drag = clone.AddComponent<Dragable>();
            drag.isDragged = true;

            // Bind it to FYFY
            GameObjectManager.bind(clone);

            // Set postion to mouse position
            clone.transform.position = point;

            // De-activate card
            // go.transform.SetParent(_players.First().transform);
        }
    }

    private void MoveCardsToHolder(GameObject holder, List<GameObject> cards)
    {
        foreach (GameObject card in cards)
        {
            // Activate card and set it as an in-game card
            card.SetActive(true);
            card.GetComponent<Card>().inGame = true;

            // Add it to the holder
            card.transform.SetParent(holder.transform);
            card.transform.localScale = new Vector3(1, 1, 1);
            card.transform.SetAsFirstSibling();
        }
    }

    private void MoveCardToGlobalDeck(GameObject card)
    {
        GameObject entity = _decks.First();
        foreach (Deck deck in entity.GetComponents<Deck>())
        {
            if (!deck.inGame)
            {
                card.transform.SetParent(entity.transform);
                deck.cards.Add(card);
            }
        }
    }
}