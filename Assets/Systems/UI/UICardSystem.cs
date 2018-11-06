using UnityEngine;
using FYFY;
using UnityEngine.UI;
using FYFY_plugins.PointerManager;

public class UICardSystem : FSystem {
    private readonly Family _holders = FamilyManager.getFamily(new AllOfComponents(typeof(CardHolder)));
    private readonly Family _cards = FamilyManager.getFamily(
        new AllOfComponents(typeof(Card)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );
    private readonly Family _hoveredCard = FamilyManager.getFamily(
        new AllOfComponents(typeof(Card), typeof(PointerOver)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );
    private readonly Family _players = FamilyManager.getFamily(new AllOfComponents(typeof(Player)));

    public UICardSystem()
    {
        _cards.addEntryCallback(InitializeCard);
        foreach (GameObject go in _cards)
        {
            InitializeCard(go);
        }
    }

    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _hoveredCard)
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
                go.transform.SetParent(_players.First().transform);
            }
        }
        foreach (GameObject go in _holders)
        {
            CardHolder holder = go.GetComponent<CardHolder>();
            if (!holder.cardAdded)
            {
                foreach (GameObject entity in _cards)
                {
                    entity.transform.SetParent(go.transform);
                    entity.transform.localScale = new Vector3(1, 1, 1);
                    entity.transform.SetAsFirstSibling();
                }
                holder.cardAdded = true;
            }
        }
	}

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
}