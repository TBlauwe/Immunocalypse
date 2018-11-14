using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;
using FYFY_plugins.TriggerManager;

public class DragAndDropSystem : FSystem {
    private readonly Family _entities = FamilyManager.getFamily(
        new AllOfComponents(typeof(Dragable)), new NoneOfComponents(typeof(Card))
    );
    private readonly Family _cards = FamilyManager.getFamily(
        new AllOfComponents(typeof(Dragable), typeof(Card))
    );

    protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _entities)
        {
            Dragable dragable = go.GetComponent<Dragable>();

            if (dragable.isDragged)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 mousePos = new Vector2();
                    Vector3 point = new Vector3();

                    // Get the mouse position from Input.
                    // Note that the y position from Input is inverted.
                    mousePos.x = Input.mousePosition.x;
                    mousePos.y = Input.mousePosition.y;

                    point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
                    point.y = 0;
                    go.transform.position = point;
                }
                else
                {
                    dragable.isDragged = false;
                    GameObjectManager.removeComponent<Dragable>(go);
                }
            }
        }

        foreach (GameObject go in _cards)
        {
            Dragable dragable = go.GetComponent<Dragable>();

            if (dragable.isDragged)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 mousePos = new Vector2
                    {
                        // Get the mouse position from Input.
                        // Note that the y position from Input is inverted.
                        x = Input.mousePosition.x,
                        y = Input.mousePosition.y
                    };

                    go.transform.position = mousePos;
                }
                else if (go.GetComponent<Triggered2D>() == null)
                {
                    dragable.isDragged = false;

                    Card card = go.GetComponent<Card>();
                    GameObject nextParent = card.lastParent;
                    card.lastParent = go.transform.parent.gameObject;

                    go.transform.SetParent(nextParent.transform);
                    go.transform.SetAsFirstSibling();
                }
            }
        }
    }
}