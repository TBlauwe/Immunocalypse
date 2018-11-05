using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;
using UnityEngine.UI;

public class CardSystem : FSystem {
    private readonly Family _cards          = FamilyManager.getFamily(new AllOfComponents(typeof(Card)));
    private readonly Family _hoveredCard    = FamilyManager.getFamily(new AllOfComponents(typeof(Card), typeof(PointerOver)));

    public CardSystem()
    {
        foreach(GameObject go in _cards)
        {
            Card card = go.GetComponent<Card>();

            Text text = (Text) go.transform.Find("Title").gameObject.GetComponent<Text>();
            text.text = card.title;

            Image image = (Image) go.transform.Find("Image").gameObject.GetComponent<Image>();
            image.sprite = card.image;
        }
    }

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
            }
        }
	}
}