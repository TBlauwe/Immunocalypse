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

    // All players (normally, only one !)
    private readonly Family _player = FamilyManager.getFamily(new AllOfComponents(typeof(Player)));

    private readonly GameObject UI = GameObject.Find("UI");

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
                GameObject entity = _player.First();
                Player player = entity.GetComponent<Player>();

                MoveCardsToHolder(go, player.levelDeck, true);
            }

            else if (go.name == "GlobalCardHolder") // Holder in preparedeck scene
            {
                GameObject entity = _player.First();
                Player player = entity.GetComponent<Player>();

                MoveCardsToHolder(go, player.globalDeck, false);
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

        // Manage triggered cards, aka cards in collision with a holder (prepare deck)
        /*foreach (GameObject go in _triggeredCards)
        {
            // Get the card component
            Card card = go.GetComponent<Card>();
            Triggered2D triggered = go.GetComponent<Triggered2D>();
            Dragable dragable = go.GetComponent<Dragable>();

            // Switch according to what type of card it is
            if (!card.lastParent.Equals(triggered.Targets[0]))
            {
                dragable.isDragged = false;
                
                // Activate card and set it as an in-game card
                go.SetActive(true);

                // Add it to the holder
                card.lastParent = triggered.Targets[0];
                go.transform.SetParent(triggered.Targets[0].transform);
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.SetAsFirstSibling();

                // Switch deck
                Player player = _player.First().GetComponent<Player>();
                if (player.globalDeck.Contains(go))
                {
                    player.globalDeck.Remove(go);
                    player.levelDeck.Add(go);
                }
                else
                {
                    player.levelDeck.Remove(go);
                    player.globalDeck.Add(go);
                }
            }

        }*/
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
            if(card.entityPrefab.GetInstanceID() == clone.GetInstanceID())
            {
                Debug.Log("good");
            }
            else
            {
                Debug.Log("bad " + card.entityPrefab.GetInstanceID());
            }
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

    private void MoveCardsToHolder(GameObject holder, List<GameObject> cards, bool inGame)
    {
        foreach (GameObject card in cards)
        {
            // Activate card and set it as an in-game card if necessary
            card.SetActive(true);
            card.GetComponent<Card>().inGame = inGame;

            // Add it to the holder
            card.transform.SetParent(holder.transform);
            card.transform.localScale = new Vector3(1, 1, 1);
            card.transform.SetAsFirstSibling();
        }
    }

    private void MoveCardToGlobalDeck(GameObject card)
    {
        // Get player
        GameObject entity = _player.First();
        Player player = entity.GetComponent<Player>();

        // Change card parent
        card.transform.SetParent(entity.transform);

        // Change card's deck
        player.globalDeck.Add(card);
        player.levelDeck.Remove(card);
    }
}